using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderMovement : MonoBehaviour
{
    // General variables
    [SerializeField] private int defaultGravity;
    private Rigidbody2D rb;
    private Animator ani;

    // Variables for movement
    [SerializeField] private int speed;
    private Vector2 currentVelocity;
    private float deltaX;

    // Variables for dashing
    [SerializeField] private float dashMultiplier;
    [SerializeField] private float maxBufferTime;
    private float dashBufferTime = 0;
    private bool horizontalBuffer;
    private bool dashBuffer;
    private bool isDashing;

    // Variables for air dashes and double jump
    [SerializeField] private uint maxAirCharges;
    [SerializeField] private float airDashSpeed;
    [SerializeField] private float timeToDoubleJump;
    private uint airCharges;



    // Variables for jumping
    [SerializeField] private int jumpSpeed;
    [SerializeField] private float maxJumpTime;
    private float jumpTime;

    private float deltaY;

    // Variables for IsGrounded method
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundCheckLayers;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        // sets currentVelocity to the velocity of the player at that instant as a "default"
        currentVelocity = rb.velocity;

        // Determines velocity based on a fixed speed value and the horizontal movement input
        deltaX = Input.GetAxis("Horizontal");

        checkDash();

        if (IsGrounded())
        {
            currentVelocity.x = deltaX * speed;
            airCharges = maxAirCharges;
        }

        // When dashing increase speed or airdash when in the air
        if (isDashing)
        {
            if (IsGrounded()) currentVelocity.x *= dashMultiplier;
            else if (airCharges > 0)
            {
                currentVelocity.x = airDashSpeed;
                airCharges--;
                dashBuffer = false;
                horizontalBuffer = false;
                isDashing = false;
            }
        }

        deltaY = Input.GetAxis("Vertical");

        // Player presses jump
        if (deltaY > 0)
        {
            // While the player is grounded they can jump
            if (IsGrounded())
            {
                currentVelocity.y = jumpSpeed;
                rb.gravityScale = 1.0f;
                jumpTime = Time.time;
            }
            // Add a bit more to the jump if the player holds the jump button after jumping
            else if ((Time.time - jumpTime) < maxJumpTime)
            {
                rb.gravityScale = 1.0f;
            }
            // Revert gravity to default
            else rb.gravityScale = defaultGravity;

            if ((Time.time - jumpTime) > (timeToDoubleJump) && airCharges > 0)
            {
                currentVelocity.y = jumpSpeed;
                rb.gravityScale = 1.0f;
                jumpTime = Time.time;
                airCharges--;
            }
        }


        // Finally convert the currentVelocity into the velocity of the player
        rb.velocity = currentVelocity;

        // When walking backwards flip
        //if (rb.velocity.x < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        // When walking forwards unflip
        //else if (rb.velocity.x > 0) transform.rotation = Quaternion.identity;

        ani.SetBool("Punch", Input.GetButtonDown("Punch"));
        ani.SetBool("Kick", Input.GetButtonDown("Kick"));
        ani.SetBool("Slash", Input.GetButtonDown("Slash"));
        ani.SetBool("Heavy Slash", Input.GetButtonDown("Heavy Slash"));
    }

    private bool IsGrounded()
    {
        // Checks for an overlap in the groundCheck and Ground Colliders
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundCheckLayers);

        // If the collider exists (isn't null) returns true, otherwise returns false
        return (collider != null);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void checkDash()
    {
        // Check if horizontal input is positive
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            // Dash when both buffers are true
            if (horizontalBuffer && dashBuffer) isDashing = true;

            // Otherwise set first buffer to true and start timer
            else
            {
                horizontalBuffer = true;
                dashBufferTime = maxBufferTime;
            }
        }

        // If horizontal input is 0 stop dashing and set second buffer to true
        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            isDashing = false;
            dashBuffer = true;
        }

        // Decrease time from Buffer by time that has passed
        dashBufferTime -= Time.deltaTime;
        if (dashBufferTime <= 0)
        {
            horizontalBuffer = false;
            dashBuffer = false;
            dashBufferTime = 0;
        }
    }
}
