using UnityEngine;


namespace PlayerController
{
    public class Scr_PlayerGroundMovement : MonoBehaviour
    {
        #region Variables
        [Header("Edit")]
        //Ground
        public float moveSpeed;
        public float crouchDrag;
        //Jump
        public float jumpForce;
        public float airDrag;

        [Header("Current Input States")]
        [SerializeField] private bool isRight;
        [SerializeField] private bool isLeft;
        [SerializeField] private bool isDown;
        [SerializeField] private bool aIsPressed;

        [Header("Current Player States")]
        [SerializeField] private bool isGrounded;

        [Header("States Restriction")]
        public bool canRun = true;
        public bool canCrouch = true;
        public bool canJump = true;

        [Header("Debug")]
        [SerializeField] private Vector2 AxisValues;

        //Hidden
        private Rigidbody2D playerRb;
        private Animator playerAnimator;
        private float lastVelocity; //For Idle Animation
        #endregion

        private void Start()
        {
            playerRb = GetComponent<Rigidbody2D>();
            playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            GetInput();
            StatesManager();
        }


        private void GetInput()
        {
            //Debug
            AxisValues = new Vector2(Input.GetAxis("P1_Horizontal"), Input.GetAxis("P1_Vertical"));

            //Input States
            isRight = Input.GetAxis("P1_Horizontal") >= 1f;
            isLeft = Input.GetAxis("P1_Horizontal") <= -1f;

            isDown = Input.GetAxis("P1_Vertical") <= -1f;

            aIsPressed = Input.GetButtonDown("P1_A");
        }

        

        private void StatesManager()
        {
            if (isGrounded)
            {
                if (!isRight && !isLeft && !isDown)
                {
                    IdleState();
                }
                else if (isDown && canCrouch)
                {
                    CrouchState();
                }

                if (aIsPressed && canJump && !isDown)
                {
                    JumpState();
                }
            }

            if ((isRight || isLeft) && !isDown && canRun)
            {
                RunState();
            }
        }

        private void IdleState()
        {
            //Behavior
            if (playerRb.velocity.x != 0f)
            {
               lastVelocity = playerRb.velocity.x;
            }
            playerRb.velocity = new Vector2(0f, playerRb.velocity.y) ;

            //Anim
            if (lastVelocity > 0f)
            {
                playerAnimator.Play("ACl_PlayerIdle");
            }
            else if (lastVelocity < 0f)
            {
                playerAnimator.Play("ACl_PlayerIdleFlip");
            }
            else
            {
                playerAnimator.Play("ACl_PlayerIdle");
            }
            
        }

        private void RunState()
        {
            if (isRight)
            {
                //Behavior
                playerRb.velocity = new Vector2(moveSpeed, playerRb.velocity.y);

                //Anim
                if (isGrounded)
                {
                    playerAnimator.Play("ACl_PlayerRun");
                }
                else
                {
                    if (playerRb.velocity.y > 0f)
                    {
                        playerAnimator.Play("ACl_PlayerJump");
                    }
                    else
                    {
                        playerAnimator.Play("ACl_PlayerFall");
                    }
                }
                
            }
            else
            {
                //Behavior
                playerRb.velocity = new Vector2(-moveSpeed, playerRb.velocity.y);

                //Anim
                if (isGrounded)
                {
                    playerAnimator.Play("ACl_PlayerRunFlip");
                }
                else
                {
                    if (playerRb.velocity.y > 0f)
                    {
                        playerAnimator.Play("ACl_PlayerJumpFlip");
                    }
                    else
                    {
                        playerAnimator.Play("ACl_PlayerFallFlip");
                    }
                }
                
            }
        }

        private void CrouchState()
        {
            //Behavior
            playerRb.drag = crouchDrag;

            //Anim
            if (isRight)
            {
                playerAnimator.Play("ACl_PlayerCrouch");
            }
            else if (isLeft)
            {
                playerAnimator.Play("ACl_PlayerCrouchFlip");
            }
            else
            {
                if (lastVelocity > 0f)
                {
                    playerAnimator.Play("ACl_PlayerCrouch");
                }
                else if (lastVelocity < 0f)
                {
                    playerAnimator.Play("ACl_PlayerCrouchFlip");
                }
                else
                {
                    playerAnimator.Play("ACl_PlayerCrouch");
                }
            }
        }

        private void JumpState()
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        #region collisonEvents
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.gameObject.layer == 8 && !isGrounded)
            {
                isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.collider.gameObject.layer == 8 && isGrounded)
            {
                isGrounded = false;
            }
        }
        #endregion

    }
}

