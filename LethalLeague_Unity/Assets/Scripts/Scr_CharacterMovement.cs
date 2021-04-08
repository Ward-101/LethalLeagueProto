using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Scr_CharacterMovement : MonoBehaviour
{
    [Header("Edit")]
    [SerializeField] private float speed = 9f;
    [SerializeField] private float slideDeceleration = 70f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float lowJumpGravityModifier = 1f;
    [SerializeField] private float fallGravityModifier = 1f;
    [SerializeField] private float forceFallGravityModifier = 1f;

    [Header("Inputs")]
    [SerializeField] private float horizontalInput;
    [SerializeField] private bool jumpInput;
    [SerializeField] private float verticalInput;

    [Header("States")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isIdle;
    [SerializeField] private bool isRun;
    [SerializeField] private bool isCrouch;
    [SerializeField] private bool isWallRide;


    private BoxCollider2D boxCollider;
    private Vector2 velocity;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        GetInput();

        Jump();
        Run();
        Crouch();

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

        jumpInput = Input.GetButton("P1_A");

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

    }

    private void Idle()
    {
        if (!isRun && !isCrouch && !isWallRide && !isJumping && !isFalling)
        {
            isIdle = true;
        }
        else
        {
            isIdle = false;
        }
    }

    private void Run()
    {
        if (!isCrouch)
        {
            isRun = true;
            velocity.x = speed * horizontalInput;
        }
        else
        {
            isRun = false;
        }
    }

    private void Crouch()
    {
        if (isGrounded && verticalInput == -1f)
        {
            isCrouch = true;

            velocity.x = Mathf.MoveTowards(velocity.x, 0, slideDeceleration * Time.deltaTime);
        }
        else
        {
            isCrouch = false;
        }
    }

    private void Jump()
    {
        if (isGrounded && !isCrouch)
        {
            velocity.y = 0;

            if (jumpInput)
            {
                isGrounded = false;
                velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y * gravityScale));
            }
        }

        if (!isGrounded)
        {
            float gravityModifier;

            if (velocity.y < 0)
            {
                gravityModifier = fallGravityModifier;
            }
            else if (velocity.y > 0 && !jumpInput)
            {
                gravityModifier = lowJumpGravityModifier;
            }
            else
            {
                gravityModifier = 1f;
            }

            if (verticalInput == -1f)
            {
                gravityModifier = forceFallGravityModifier;
            }

            velocity.y += Physics2D.gravity.y * gravityScale * gravityModifier * Time.deltaTime;
        }

    }

    private void CollisionDetection()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

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

                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                {
                    isGrounded = true;
                }

                if (Vector2.Angle(colliderDistance.normal, Vector2.left) < 90 && horizontalInput == -1 && !isGrounded)
                {
                    isWallRide = true;
                }
                else
                {
                    isWallRide = false;
                }
            }
        }
    }
}
