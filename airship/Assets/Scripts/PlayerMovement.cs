using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    public string up;
    public string down;
    public string left;
    public string right;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        float x=0, y=0;

        if (Input.GetKey(up)) y = 1;
        if (Input.GetKey(down)) y = -1;
        if (Input.GetKey(left)) x = -1;
        if (Input.GetKey(right)) x = 1;

        // Calculate movement vector
        Vector2 movement = new Vector2(x, y).normalized;

        // Move the player
        MovePlayer(movement);

    }

    void MovePlayer(Vector2 movement)
    {
        // Apply movement to the rigidbody
        rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);

        
    }
}
