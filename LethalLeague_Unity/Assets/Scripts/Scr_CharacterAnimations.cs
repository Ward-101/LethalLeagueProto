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
            else if (movement.isHorizontalMovement && movement.isGrounded)
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
        if (movement.lastXVelocity >= 0f)
        {
            animator.Play("ACl_PlayerIdle");
        }
        else
        {
            animator.Play("ACl_PlayerIdleFlip");
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
            if (movement.lastXVelocity >= 0f)
            {
                animator.Play("ACl_PlayerCrouch");
            }
            else
            {
                animator.Play("ACl_PlayerCrouchFlip");
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
                if (movement.lastXVelocity >= 0f)
                {
                    animator.Play("ACl_PlayerJump");
                }
                else
                {
                    animator.Play("ACl_PlayerJumpFlip");
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
                if (movement.lastXVelocity >= 0f)
                {
                    animator.Play("ACl_PlayerFall");
                }
                else
                {
                    animator.Play("ACl_PlayerFallFlip");
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

    #region Attack
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
        if (attack.isStartup)
        {
            if (attack.attackDir >= 0f)
            {
                animator.Play("ACl_PlayerNormalStartup");
            }
            else
            {
                animator.Play("ACl_PlayerNormalStartupFlip");
            }
        }
        else if (attack.isActive)
        {
            if (attack.attackDir >= 0f)
            {
                animator.Play("ACl_PlayerNormalActive");
            }
            else
            {
                animator.Play("ACl_PlayerNormalActiveFlip");
            }

        }
        else if (attack.isRecovery)
        {
            if (attack.attackDir >= 0f)
            {
                animator.Play("ACl_PlayerNormalRecovery");
            }
            else
            {
                animator.Play("ACl_PlayerNormalRecoveryFlip");
            }
        }
    }

    private void Smash()
    {
        if (attack.isStartup)
        {
            if (attack.attackDir >= 0f)
            {
                animator.Play("ACl_PlayerSmashStartup");
            }
            else
            {
                animator.Play("ACl_PlayerSmashStartupFlip");
            }
        }
        else if (attack.isActive)
        {
            if (attack.attackDir >= 0f)
            {
                animator.Play("ACl_PlayerSmashActive");
            }
            else
            {
                animator.Play("ACl_PlayerSmashActiveFlip");
            }

        }
        else if (attack.isRecovery)
        {
            if (attack.attackDir >= 0f)
            {
                animator.Play("ACl_PlayerSmashRecovery");
            }
            else
            {
                animator.Play("ACl_PlayerSmashRecoveryFlip");
            }

        }
    }

    private void Lob()
    {
        if (attack.isStartup)
        {
            if (attack.attackDir >= 0f)
            {
                animator.Play("ACl_PlayerLobStartup");
            }
            else
            {
                animator.Play("ACl_PlayerLobStartupFlip");
            }
        }
        else if (attack.isActive)
        {
            if (attack.attackDir >= 0f)
            {
                animator.Play("ACl_PlayerLobActive");
            }
            else
            {
                animator.Play("ACl_PlayerLobActiveFlip");
            }

        }
        else if (attack.isRecovery)
        {
            if (attack.attackDir >= 0f)
            {
                animator.Play("ACl_PlayerLobRecovery");
            }
            else
            {
                animator.Play("ACl_PlayerLobRecoveryFlip");
            }

        }
       
    }
#endregion
}
