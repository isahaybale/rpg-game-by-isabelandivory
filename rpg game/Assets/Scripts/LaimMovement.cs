using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaimMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private Vector2 lastMoveDir = Vector2.down; // Default facing down at start

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Normalize diagonal input
        moveInput = moveInput.normalized;

        // Check if player is moving
        bool isMoving = moveInput.sqrMagnitude > 0;

        if (isMoving)
        {
            lastMoveDir = moveInput; // Remember last direction
        }

        // Update animator
        if (animator != null)
        {
            animator.SetFloat("X", isMoving ? moveInput.x : lastMoveDir.x);
            animator.SetFloat("Y", isMoving ? moveInput.y : lastMoveDir.y);
            animator.SetFloat("Speed", moveInput.sqrMagnitude);
            animator.SetBool("IsMoving", isMoving);
        }
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
