using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;
    public PlayerRoll playerRoll;

    public float speed;
    private Vector2 movement;
    private Vector2 movementBase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (!playerRoll.GetRolling())
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");

            Vector2 moveInput = new Vector2(moveHorizontal, moveVertical);

            movementBase = moveInput.normalized * Time.fixedDeltaTime;
            movement = movementBase * speed;

            animator.SetFloat("speed", (float)Math.Pow(movement.magnitude, 2));
            rb.MovePosition(rb.position + movement);
        }

        if (playerRoll.GetRolling())
        {
            movement = movementBase * speed;

            animator.SetFloat("speed", (float)Math.Pow(movement.magnitude, 2));
            rb.MovePosition(rb.position + movement);
        }

        animator.SetFloat("speedX", movementBase.x);
        animator.SetFloat("speedY", movementBase.y);
    }

    public Vector2 GetMovement()
    {
        return movement;
    }

    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    
    public float getSpeed()
    {
        return speed;
    }
}
