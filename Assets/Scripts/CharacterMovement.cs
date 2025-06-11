using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterMovement : NetworkBehaviour
{
    // General
    [SerializeField] private int defaultGravity;
    private Vector2 currentVelocity;
    private Rigidbody2D rb;
    private Animator ani;

    // Ground Movement
    [SerializeField] private uint forwardSpeed;
    [SerializeField] private uint backwardSpeed;
    private float deltaX;
    private bool isFlipped;

    // Jumping
    [SerializeField] private uint jumpingSpeed;

    // IsGrounded
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundCheckLayers;

    public PlayerControlls playerControlls { get; private set; }

    private void Awake()
    {
        playerControlls = new PlayerControlls();
    }

    private void OnEnable()
    {
        playerControlls.Enable();
    }

    private void OnDisable()
    {
        playerControlls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();

        isFlipped = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentVelocity = rb.linearVelocity;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isFlipped)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                isFlipped = true;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                isFlipped = false;
            }
        }

        deltaX = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            switch ((deltaX, Input.GetAxisRaw("Vertical")))
            {
                case ( > 0, 0):
                    RWalk();
                    break;
                case ( < 0, 0):
                    LWalk();
                    break;
                case (0, > 0):
                    Jump();
                    break;
                case ( > 0, > 0):
                    Jump();
                    break;
                case ( < 0, > 0):
                    Jump();
                    break;
                case (0, < 0):
                    Crouch();
                    break;
                case ( < 0, < 0):
                    Crouch();
                    break;
                default:
                    Idle();
                    break;
            }

            rb.linearVelocity = currentVelocity;
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
    private void RWalk()
    {
        if (!isFlipped)
        {
            currentVelocity.x = forwardSpeed;
            Debug.Log("Walking Forward.");
        }
        else
        {
            currentVelocity.x = backwardSpeed;
            Debug.Log("Walking Backwards.");
            Debug.Log("Blocking High.");
        }
    }
    private void LWalk()
    {
        if (!isFlipped)
        {
            currentVelocity.x = -backwardSpeed;
            Debug.Log("Walking Backwards.");
            Debug.Log("Blocking High.");
        }
        else
        {
            currentVelocity.x = -forwardSpeed;
            Debug.Log("Walking Forward.");
        }
        
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

        //if (inputManager.playerControlls.Player.Right.triggered)
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!isFlipped)
            {
                currentVelocity.x = forwardSpeed / 2;
                Debug.Log("Jumping Forward.");
            }
            else
            {
                currentVelocity.x = backwardSpeed / 2;
                Debug.Log("Jumping Backwards.");
            }
        }
        //else if (inputManager.playerControlls.Player.Left.triggered)
            else if (Input.GetKeyDown(KeyCode.A))

            {
                if (!isFlipped)
            {
                currentVelocity.y = jumpingSpeed;
                currentVelocity.x = -backwardSpeed / 2;
                Debug.Log("Jumping Backwards.");
            }
            else
            {
                currentVelocity.y = jumpingSpeed;
                currentVelocity.x = -forwardSpeed / 2;
                Debug.Log("Jumping Forward.");
            }
        }
    }

    private void Crouch()
    {
        ani.SetBool("Crouching", Input.GetButtonDown("Heavy Slash"));
        Debug.Log("Crouching.");
        //if (inputManager.playerControlls.Player.Left.triggered && !isFlipped)
        if (Input.GetKeyDown(KeyCode.A) && !isFlipped)
        {
            Debug.Log("Blocking Low");
        }
        //else if (inputManager.playerControlls.Player.Right.triggered && isFlipped)
        else if (Input.GetKeyDown(KeyCode.D) && isFlipped)
        {
            Debug.Log("Blocking Low");
        }
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
