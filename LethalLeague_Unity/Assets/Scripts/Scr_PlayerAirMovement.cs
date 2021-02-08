using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
    public class Scr_PlayerAirMovement : MonoBehaviour
    {
        [Header("Current Player States")]
        public bool isGrounded = false;

        [Header("State Restrictions")]
        public bool canJump;


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

        
    }
}

