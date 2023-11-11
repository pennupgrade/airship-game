using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, 0.2f, groundLayer);

        // Input handling
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Calculate movement vector
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;

        // Move the player
        MovePlayer(movement);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void MovePlayer(Vector2 movement)
    {
        // Apply movement to the rigidbody
        rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);

        
    }

    void Jump()
    {
        // Apply an upward force to simulate jumping
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
