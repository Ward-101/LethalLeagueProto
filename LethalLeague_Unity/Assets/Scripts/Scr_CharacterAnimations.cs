using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Scr_CharacterMovement))]
public class Scr_CharacterAnimations : MonoBehaviour
{
    [Header("Edit")]
    [SerializeField, Range(0f, 10f)] private float fallAnimCorrection = 0f;

    private Animator animator;
    private Scr_CharacterMovement movement;

    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<Scr_CharacterMovement>();
    }

    private void Update()
    {
        MovementAnimation();
    }

    private void MovementAnimation()
    {
        if (movement.isIdle && movement.isGrounded)
        {
            Idle();
        }
        else if (movement.isRun && movement.isGrounded)
        {
            Run();
        }
        else if (movement.isWallRide)
        {
            WallRide();
        }
        else if (!movement.isGrounded)
        {
            Jump();
        }
        else if (movement.isCrouch)
        {
            Crouch();
        }
          
    }

    private void Idle()
    {
        if (movement.lastXVelocity > 0f)
        {
            animator.Play("ACl_PlayerIdle");
        }
        else if (movement.lastXVelocity < 0)
        {
            animator.Play("ACl_PlayerIdleFlip");
        }
        else
        {
            animator.Play("ACl_PlayerIdle");
        }
    }

    private void Run()
    {
        if (movement.velocity.x > 0)
        {
            animator.Play("ACl_PlayerRun");
        }
        else
        {
            animator.Play("ACl_PlayerRunFlip");
        }
    }

    private void Crouch()
    {
        if (movement.horizontalInput > 0)
        {
            animator.Play("ACl_PlayerCrouch");
        }
        else if (movement.horizontalInput < 0)
        {
            animator.Play("ACl_PlayerCrouchFlip");
        }
        else
        {
            if (movement.lastXVelocity > 0f)
            {
                animator.Play("ACl_PlayerCrouch");
            }
            else if (movement.lastXVelocity < 0f)
            {
                animator.Play("ACl_PlayerCrouchFlip");
            }
            else
            {
                animator.Play("ACl_PlayerCrouch");
            }
        }
    }

    private void Jump()
    {
        if (movement.velocity.y > fallAnimCorrection)
        {
            if (movement.velocity.x > 0)
            {
                animator.Play("ACl_PlayerJump");
            }
            else if (movement.velocity.x < 0)
            {
                animator.Play("ACl_PlayerJumpFlip");
            }
            else
            {
                if (movement.lastXVelocity > 0f)
                {
                    animator.Play("ACl_PlayerJump");
                }
                else if (movement.lastXVelocity < 0f)
                {
                    animator.Play("ACl_PlayerJumpFlip");
                }
                else
                {
                    animator.Play("ACl_PlayerJump");
                }
            }
        }
        else
        {
            if (movement.velocity.x > 0)
            {
                animator.Play("ACl_PlayerFall");
            }
            else if (movement.velocity.x < 0)
            {
                animator.Play("ACl_PlayerFallFlip");
            }
            else
            {
                if (movement.lastXVelocity > 0f)
                {
                    animator.Play("ACl_PlayerFall");
                }
                else if (movement.lastXVelocity < 0f)
                {
                    animator.Play("ACl_PlayerFallFlip");
                }
                else
                {
                    animator.Play("ACl_PlayerFall");
                }
            }
        }
    }

    private void WallRide()
    {
        if (movement.horizontalInput > 0)
        {
            animator.Play("ACl_PlayerWallRideFlip");
        }
        else
        {
            animator.Play("ACl_PlayerWallRide");
        }
    }
}
