using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Scr_CharacterMovement : MonoBehaviour
{
    #region Variables
    [Header("Edit")]
    [SerializeField] private float speed = 9f;
    [SerializeField] private float crouchMinDuration = 0.2f;
    [SerializeField] private float slideDeceleration = 70f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(3f, 3f);
    [SerializeField] private float wallJumpDuration = 0.5f;
    [SerializeField] private float wallRideCooldown = 0.5f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float highJumpGravityModifier = 1f;
    [SerializeField] private float fallGravityModifier = 1f;
    [SerializeField] private float forceFallGravityModifier = 1f;
    [SerializeField] private float wallRideGravityModifier = 1f;
    [SerializeField, Range(0f, -10f)] private float fallBehaviorCorrection = 0f;

    [Header("Inputs")]
    public float horizontalInput;
    [SerializeField] private bool jumpInput;
    [SerializeField] private bool wallJumpInput;
    [SerializeField] private float verticalInput;

    [Header("States")]
    public bool isGrounded;
    public bool isFalling;
    public bool isIdle;
    public bool isRun;
    public bool isCrouch;
    public bool isWallRide;
    public bool isWallJump;
    
    private BoxCollider2D boxCollider;
    [HideInInspector] public Vector2 velocity;

    [HideInInspector] public float lastXVelocity = 0f;
    private float lastHorizontalInput = 0f;

    private bool lockGravity = false;
    private bool lockCrouch = false;
    private bool canCrouch = true;
    private bool canWallRide = true;

    private float currentCrouchTime = 0f;
    private float currentWallJumpTime = 0f;
    private float currentWallRideElapsedTime = 0f;
    private float wallRideSide = 0f;
    #endregion

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        GetInput();

        Jump();
        WallRide();
        Gravity();
        Crouch();
        HorizontalMovement();
        
        transform.Translate(velocity * Time.deltaTime);

        CollisionDetection();

        Idle();
    }

    private void GetInput()
    {
        if (Input.GetAxisRaw("P1_Horizontal") >= 1f)
        {
            horizontalInput = 1f;
        }
        else if (Input.GetAxisRaw("P1_Horizontal") <= -1f)
        {
            horizontalInput = -1f;
        }
        else
        {
            horizontalInput = 0f;
        }

        if (Input.GetAxisRaw("P1_Vertical") >= 1f)
        {
            verticalInput = 1f;
        }
        else if (Input.GetAxisRaw("P1_Vertical") <= -1f)
        {
            verticalInput = -1f;
        }
        else
        {
            verticalInput = 0f;
        }

        jumpInput = Input.GetButton("P1_A");

        wallJumpInput = Input.GetButtonDown("P1_A");
    }

    private void Idle()
    {
        if (!isRun && !isCrouch && !isWallRide && !isWallJump && !isFalling)
        {
            isIdle = true;
            velocity.x = 0f;
        }
        else
        {
            isIdle = false;
        }
    }

    private void HorizontalMovement()
    {
        if (!isCrouch && !isWallRide && !isWallJump && horizontalInput != 0)
        {
            isRun = true;
            velocity.x = speed * horizontalInput;
            lastXVelocity = velocity.x;
        }
        else
        {
            isRun = false;
        }
    }

    private void Crouch()
    {
        if (!canCrouch && verticalInput != -1f)
        {
            canCrouch = true;
        }

        if ((isGrounded && verticalInput == -1f && canCrouch) || lockCrouch)
        {
            isCrouch = true;
            lockCrouch = true;

            currentCrouchTime += Time.deltaTime;

            if(currentCrouchTime >= crouchMinDuration)
            {
                lockCrouch = false;
            }

            velocity.x = Mathf.MoveTowards(velocity.x, 0f, slideDeceleration * Time.deltaTime);
        }
        else
        {
            currentCrouchTime = 0f;
            isCrouch = false;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            velocity.y = 0;

            if (jumpInput && !isCrouch)
            {
                isGrounded = false;
                velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y * gravityScale));
            }
        }

        if ((wallJumpInput && isWallRide) || isWallJump)
        {
            if (!isWallJump)
            {
                lastHorizontalInput = horizontalInput;
                isWallJump = true;
                isWallRide = false;
            }

            currentWallJumpTime += Time.deltaTime;

            if (currentWallJumpTime >= wallJumpDuration)
            {
                isWallJump = false;
                currentWallJumpTime = 0f;
            }

            velocity.y = Mathf.Sqrt(2 * wallJumpForce.y * Mathf.Abs(Physics2D.gravity.y * gravityScale));
            velocity.x = wallJumpForce.x * -lastHorizontalInput;
        }
    }

    private void WallRide()
    {
        if (isWallRide)
        {
            if (wallRideSide != horizontalInput)
            {
                isWallRide = false;
                canWallRide = false;
            }
        }

        if (!canWallRide)
        {
            currentWallRideElapsedTime += Time.deltaTime;

            if (currentWallRideElapsedTime >= wallRideCooldown)
            {
                canWallRide = true;
                currentWallRideElapsedTime = 0f;
            }
        }
    }

    private void Gravity()
    {
        if (!isGrounded)
        {
            if (velocity.y > fallBehaviorCorrection)
            {
                isFalling = false;
            }
            else
            {
                isFalling = true;
            }

            float gravityModifier;

            if (isWallRide && !lockGravity)
            {
                gravityModifier = wallRideGravityModifier;
            }
            else if ((verticalInput == -1f && horizontalInput == 0 && isFalling) || lockGravity)
            {
                gravityModifier = forceFallGravityModifier;
                lockGravity = true;
                canCrouch = false;
            }
            else if (isFalling)
            {
                gravityModifier = fallGravityModifier;
            }
            else if (!isFalling && jumpInput)
            {
                gravityModifier = highJumpGravityModifier;
            }
            else
            {
                gravityModifier = 1f;
            }

            velocity.y += Physics2D.gravity.y * gravityScale * gravityModifier * Time.deltaTime;
        }
    }

    private void CollisionDetection()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0f);

        foreach (Collider2D hit in hits)
        {
            if (hit == boxCollider)
            {
                continue;
            }

            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

            if (colliderDistance.isOverlapped)
            {
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                if (Vector2.Angle(colliderDistance.normal, Vector2.up) == 0f && velocity.y < 0f && !isGrounded)
                {
                    velocity.y = 0;

                    isWallRide = false;
                    isGrounded = true;
                    isFalling = false;
                    lockGravity = false;

                }

                if (((Vector2.Angle(colliderDistance.normal, Vector2.right) == 0f && horizontalInput == -1f) || (Vector2.Angle(colliderDistance.normal, Vector2.left) == 0f && horizontalInput == 1f)) && !isGrounded && velocity.y < 0f && !isWallRide && canWallRide)
                {
                    wallRideSide = horizontalInput;

                    if (!isWallRide)
                    {
                        velocity = Vector2.zero;
                    }

                    isWallRide = true;
                }
            }
        }
    }
}
