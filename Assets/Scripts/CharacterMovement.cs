using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // General variables
    [SerializeField] private int defaultGravity;
    private Vector2 currentVelocity;
    private Rigidbody2D rb;
    private Animator ani;

    // Ground Movement
    [SerializeField] private uint forwardSpeed;
    [SerializeField] private uint backwardSpeed;
    private float deltaX;
    private int isFlipped;

    // Jumping
    [SerializeField] private uint jumpingSpeed;

    // IsGrounded
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundCheckLayers;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();

        isFlipped = 1;
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity = rb.velocity;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isFlipped > 0) transform.rotation = Quaternion.Euler(0, 180, 0);
            else transform.rotation = Quaternion.Euler(0, 0, 0);
            isFlipped = isFlipped * -1;
        }

        deltaX = Input.GetAxisRaw("Horizontal");
        if (isFlipped < 0) deltaX = -deltaX;

        if (IsGrounded())
        {
            switch ((deltaX, Input.GetAxisRaw("Vertical")))
            {
                case ( > 0, 0):
                    FWalk();
                    break;
                case ( < 0, 0):
                    BWalk();
                    break;
                case (0, > 0):
                    Jump();
                    break;
                case ( > 0, > 0):
                    FJump();
                    break;
                case ( < 0, > 0):
                    BJump();
                    break;
                case (0, < 0):
                    Crouch();
                    break;
                case ( < 0, < 0):
                    BCrouch();
                    break;
                default:
                    Idle();
                    break;
            }

            rb.velocity = currentVelocity;
        }
    }

    private bool IsGrounded()
    {
        // Checks for an overlap in the groundCheck and Ground Colliders
        Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundCheckLayers);

        // If the collider exists (isn't null) returns true, otherwise returns false
        return (collider != null);
    }


    // Grounded movement
    private void Idle()
    {
        
    }
    private void FWalk()
    {
        currentVelocity.x = forwardSpeed * isFlipped;
        Debug.Log("Walking Forward.");
    }
    private void BWalk()
    {
        currentVelocity.x = -backwardSpeed * isFlipped;
        Debug.Log("Walking Backwards.");
        Debug.Log("Blocking High.");
    }
    private void Dash()
    {
        Debug.Log("Dashing Forward.");
    }
    private void BDash()
    {
        Debug.Log("Dahsing Backwards.");
    }
    private void Jump()
    {
        currentVelocity.y = jumpingSpeed;
        Debug.Log("Jumping Up.");
    }
    private void FJump()
    {
        currentVelocity.y = jumpingSpeed;
        currentVelocity.x = forwardSpeed * isFlipped / 2;
        Debug.Log("Jumping Forward.");
    }
    private void BJump()
    {
        currentVelocity.y = jumpingSpeed;
        currentVelocity.x = -backwardSpeed * isFlipped / 2;
        Debug.Log("Jumping Backwards.");
    }
    private void Crouch()
    {
        Debug.Log("Crouching.");
    }
    private void BCrouch()
    {
        Debug.Log("Crouching.");
        Debug.Log("Blocking Low");
    }


    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
