using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class Scr_PlayerAirMovement : MonoBehaviour
    {
        #region Variables
        [Header("Edit")]
        public float jumpForce;
        public float airMoveSpeed;

        [Header("Current Player States")]
        public bool isGrounded = false;

        [Header("State Restrictions")]
        public bool canJump;

        [Header("Input States")]
        public bool aPressed;
        public bool isRight;
        public bool isLeft;

        //Hidden
        private Rigidbody2D playerRb;
        private Animator playerAnimator;
        private float lastVelocity;

        #endregion

        private void Start()
        {
            playerRb = GetComponent<Rigidbody2D>();
            playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            GetInput();
            StateManager();
        }


        private void GetInput()
        {
            aPressed = Input.GetButtonDown("P1_A");

            isRight = Input.GetAxis("P1_Horizontal") >= 1f;
            isLeft = Input.GetAxis("P1_Horizontal") <= -1f;
        }

        private void StateManager()
        {
            if (aPressed && isGrounded)
            {
                JumpState();
            }
            else if (!isGrounded)
            {
                MoveState();
            }
        }

        private void JumpState()
        {
            JumpEvent();

            if (!isGrounded)
            {

                if (playerRb.velocity.y > 0f)
                {
                    if (isRight)
                    {
                        playerAnimator.Play("ACl_PlayerJump");
                    }
                    else
                    {
                        playerAnimator.Play("ACl_PlayerJumpFlip");
                    }

                    
                }
                else
                {
                    if (isRight)
                    {
                        playerAnimator.Play("ACl_PlayerFall");
                    }
                    else
                    {
                        playerAnimator.Play("ACl_PlayerFallFlip");
                    }
                    
                }
            }
        }

        private void MoveState()
        {
            if (isRight)
            {
                //Behavior
                playerRb.velocity = new Vector2(airMoveSpeed, playerRb.velocity.y);
            }
            else
            {
                //Behavior
                playerRb.velocity = new Vector2(-airMoveSpeed, playerRb.velocity.y);
            }
        }

        private void JumpEvent()
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

