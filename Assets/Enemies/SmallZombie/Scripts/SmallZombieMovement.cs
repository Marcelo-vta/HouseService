using System;
using System.Collections;
using UnityEngine;

public class SmallZombieMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;

    private Animator animator;
    public SmallZombieLongAttack smallZombieLongAttack;
    private Vector2 movement;
    private Vector2 movementBase;
    private Vector2 lastDirection;
    private float lastXVel;
    private float lastYVel;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!smallZombieLongAttack.GetLongAttacking())
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");

            Vector2 moveInput = new Vector2(moveHorizontal, moveVertical);

            movementBase = moveInput.normalized * Time.fixedDeltaTime;
            
            if (moveInput.x != 0 || moveInput.y != 0)
            {
                lastDirection = new Vector2(moveInput.x, moveInput.y).normalized;
                lastXVel = lastDirection.x;
                lastYVel = lastDirection.y;
            }
        }
        else
        {
            movementBase = lastDirection * Time.fixedDeltaTime;
        }

        if (movementBase.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementBase.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        movement = movementBase * speed;
        if (smallZombieLongAttack.GetLongAttacking())
        {
            animator.SetFloat("Vel", 0);
        }
        else
        {
            animator.SetFloat("Vel", movement.magnitude * 100f);
        }
        rb.MovePosition(rb.position + movement);

        animator.SetFloat("xVel", movementBase.x);
        animator.SetFloat("yVel", movementBase.y);
        animator.SetFloat("lastXVel", lastXVel);
        animator.SetFloat("lastYVel", lastYVel);
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
