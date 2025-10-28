using System;
using System.Collections;
using UnityEngine;

public class SmallZombieScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;

    private bool isLongAttacking = false;

    private Animator animator;

    private float attackOffsetX = 1.5f;
    private float attackOffsetY = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();    
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isLongAttacking)
        {
            HandleLongAttack();
        }

        if (isLongAttacking)
        {
            animator.SetFloat("Vel", 0);
        }
        else
        {
            HandleMovement();
        }
    }

    // This method should be called as an animation event at the end of the attack animation.
    // This method should be called as an animation event at the end of the attack animation.
    public void AttackFinished()
    {
        isLongAttacking = false;
        animator.SetBool("isLongAttacking", false);
        Vector3 pos = transform.position;

        float lastX = animator.GetFloat("lastXVel");
        float lastY = animator.GetFloat("lastYVel");
        bool appliedOffset = false; // Flag to check if we need to update position

        // Check if horizontal movement is dominant (or equal)
        if (Mathf.Abs(lastX) >= Mathf.Abs(lastY))
        {
            if (lastX < 0)
            {
                pos.x -= attackOffsetX;
                appliedOffset = true;
            }
            else if (lastX > 0)
            {
                pos.x += attackOffsetX;
                appliedOffset = true;
            }
        }
        // Else, vertical movement is dominant
        else
        {
            if (lastY < 0)
            {
                pos.y -= attackOffsetY; // Use attackOffsetY here
                appliedOffset = true;
            }
            else if (lastY > 0)
            {
                pos.y += attackOffsetY; // Use attackOffsetY here
                appliedOffset = true;
            }
        }

        // Only update position if an offset was actually calculated
        if (appliedOffset)
        {
            StartCoroutine(WaitAndUpdatePosition(pos));
        }
    }

    private IEnumerator WaitAndUpdatePosition(Vector3 pos)
    {
        yield return null; // wait for 1 frame
        transform.position = pos;
    }

    private void HandleMovement()
    {
        // ... (This method remains unchanged)
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("xVel", moveHorizontal);

        float moveVertical = Input.GetAxisRaw("Vertical");
        animator.SetFloat("yVel", moveVertical);

        if (moveHorizontal != 0 || moveVertical != 0)
        {
            animator.SetFloat("lastXVel", moveHorizontal);
            animator.SetFloat("lastYVel", moveVertical);
        }


        animator.SetFloat("Vel", Math.Abs(moveHorizontal) + Math.Abs(moveVertical));

        Vector2 moveInput = new Vector2(moveHorizontal, moveVertical);

        rb.MovePosition(rb.position + moveInput.normalized * speed * Time.fixedDeltaTime);
    }

    
    private void HandleLongAttack()
    {
        isLongAttacking = true;
        animator.SetBool("isLongAttacking", true);
        Vector3 pos = transform.position;

        // --- Apply the same logic from AttackFinished ---
        float lastX = animator.GetFloat("lastXVel");
        float lastY = animator.GetFloat("lastYVel");
        bool appliedOffset = false; // Flag to check if we need to update position

        // Check if horizontal movement is dominant (or equal)
        if (Mathf.Abs(lastX) >= Mathf.Abs(lastY))
        {
            if (lastX < 0)
            {
                pos.x -= attackOffsetX;
                appliedOffset = true;
            }
            else if (lastX > 0)
            {
                pos.x += attackOffsetX;
                appliedOffset = true;
            }
        }
        // Else, vertical movement is dominant
        else
        {
            if (lastY < 0)
            {
                pos.y -= attackOffsetY; // Use attackOffsetY here
                appliedOffset = true;
            }
            else if (lastY > 0)
            {
                pos.y += attackOffsetY; // Use attackOffsetY here
                appliedOffset = true;
            }
        }

        // Only update position if an offset was actually calculated
        if (appliedOffset)
        {
            StartCoroutine(WaitAndUpdatePosition(pos));
        }
    }
}
