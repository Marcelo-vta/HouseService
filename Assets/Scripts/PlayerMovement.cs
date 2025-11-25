using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed;

    private PlayerStates playerStates;
    private Rigidbody2D rb;
    private Animator spriteAnimator;

    private Vector2 movement;
    private Vector2 movementBase;
    private float moveHorizontal;
    private float moveVertical;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStates = GetComponent<PlayerStates>();
        spriteAnimator = GetComponentInChildren<Animator>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = InputManager.Instance.MoveInput;
        moveHorizontal = input.x;
        moveVertical = input.y;

        playerStates.walkingState = MathF.Abs(moveHorizontal) + MathF.Abs(moveVertical) > 0;

    }

    void FixedUpdate()
    {
        if (playerStates.ableToWalk)
        {
            Vector2 moveInput = new Vector2(moveHorizontal, moveVertical);

            movementBase = moveInput.normalized * Time.fixedDeltaTime;
            movement = movementBase * speed;

            spriteAnimator.SetFloat("speed", (float)Math.Pow(movement.magnitude, 2));
            rb.MovePosition(rb.position + movement);

            spriteAnimator.SetFloat("speedX", movementBase.x);
            spriteAnimator.SetFloat("speedY", movementBase.y);

            if (movementBase.x != 0)
            {
                spriteAnimator.SetFloat("last_speed_X", movementBase.x);
            }
            
            if (movementBase.y != 0)
            {
                spriteAnimator.SetFloat("last_speed_Y", movementBase.y);
            }
        }

        if (playerStates.rollingState)
        {
            movement = movementBase * speed;

            spriteAnimator.SetFloat("speed", (float)Math.Pow(movement.magnitude, 2));
            rb.MovePosition(rb.position + movement);
        }
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
