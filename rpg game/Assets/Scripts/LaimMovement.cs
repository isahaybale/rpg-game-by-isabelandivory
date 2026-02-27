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
        // ✅ If movement is locked, force input to zero and update animator as idle
        if (!GameState.CanPlayerMove)
        {
            moveInput = Vector2.zero;

            if (animator != null)
            {
                animator.SetFloat("X", lastMoveDir.x);
                animator.SetFloat("Y", lastMoveDir.y);
                animator.SetFloat("Speed", 0f);
                animator.SetBool("IsMoving", false);
            }
            return;
        }

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
        // ✅ Don’t move if locked
        if (!GameState.CanPlayerMove) return;

        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}