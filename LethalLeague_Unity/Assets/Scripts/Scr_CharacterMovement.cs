using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Scr_CharacterMovement : MonoBehaviour
{
    [Header("Edit")]
    [SerializeField] private float speed = 9;
    [SerializeField] private float slideDeceleration = 70;
    [SerializeField] private float jumpHeight = 4;
    [SerializeField] private float gravityScale = 1;

    [Header("Inputs")]
    [SerializeField] private float horizontalInput;
    [SerializeField] private bool jumpInput;
    [SerializeField] private float verticalInput;

    [Header("States")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isRun;
    [SerializeField] private bool isCrouch;


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

        jumpInput = Input.GetButtonDown("P1_A");

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
                velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y * gravityScale));
            }
        }

        velocity.y += Physics2D.gravity.y * gravityScale * Time.deltaTime;
    }

    private void CollisionDetection()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);
        isGrounded = false;

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
            }
        }
    }
}
