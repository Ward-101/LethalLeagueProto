using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Scr_CharacterMovement), typeof(Scr_CharacterAttack))]
public class Scr_CharacterAnimations : MonoBehaviour
{
    #region Variables
    [Header("Edit")]
    [SerializeField, Range(0f, 10f)] private float fallAnimCorrection = 0f;

    private Animator animator;
    private Scr_CharacterMovement movement;
    private Scr_CharacterAttack attack;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<Scr_CharacterMovement>();
        attack = GetComponent<Scr_CharacterAttack>();
    }

    private void Update()
    {
        AttackAnimation();
        MovementAnimation();
    }

    #region Movement
    private void MovementAnimation()
    {

        if (!attack.isNormal && !attack.isSmash && !attack.isLob)
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
        if (movement.velocity.y > fallAnimCorrection || movement.isWallJump)
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
    #endregion

    private void AttackAnimation()
    {
        if (attack.isNormal && !attack.isSmash && !attack.isLob)
        {
            Normal();
        }
        else if (attack.isSmash && !attack.isNormal && !attack.isLob)
        {
            Smash();
        }
        else if (attack.isLob && !attack.isNormal && !attack.isSmash)
        {
            Lob();
        }
    }

    private void Normal()
    {
   
    }

    private void Smash()
    {

    }

    private void Lob()
    {
        if (attack.isStartup)
        {
            if (attack.attackDir > 0f)
            {
                animator.Play("ACl_PlayerLobStartup");
            }
            else if (attack.attackDir < 0f)
            {
                animator.Play("ACl_PlayerLobStartupFlip");
            }
            else
            {
                animator.Play("ACl_PlayerLobStartup");
            }
        }
        else if (attack.isActive)
        {
            if (attack.attackDir > 0f)
            {
                animator.Play("ACl_PlayerLobActive");
            }
            else if (attack.attackDir < 0f)
            {
                animator.Play("ACl_PlayerLobActiveFlip");
            }
            else
            {
                animator.Play("ACl_PlayerLobActive");
            }
        }
        else if (attack.isRecovery)
        {
            if (attack.attackDir > 0f)
            {
                animator.Play("ACl_PlayerLobRecovery");
            }
            else if (attack.attackDir < 0f)
            {
                animator.Play("ACl_PlayerLobRecoveryFlip");
            }
            else
            {
                animator.Play("ACl_PlayerLobRecovery");
            }
        }
       
    }
}
