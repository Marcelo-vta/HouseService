using System;
using System.Collections;
using UnityEngine;
public class BigZombieScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;

    private Boolean isAttacking = false;
    private Animator animator;

    private float attackOffsetX = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            Vector3 pos = transform.position;

            if (animator.GetFloat("lastXVel") < 0)
            {
                pos.x -= attackOffsetX; // Slight position adjustment to avoid clipping issues
                StartCoroutine(WaitAndUpdatePosition(pos));
            }
            else if (animator.GetFloat("lastXVel") > 0)
            {
                pos.x += attackOffsetX; // Slight position adjustment to avoid clipping issues
                StartCoroutine(WaitAndUpdatePosition(pos));
            }

            // wait for 1 frame before updating position
            
        }

        if (isAttacking)
        {
            animator.SetFloat("Vel", 0);
        }
        else
        {
            // TODO change movement to pathfinding towards player
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
    }

    private IEnumerator WaitAndUpdatePosition(Vector3 pos)
    {
        yield return null; // wait for 1 frame
        transform.position = pos;
    }

    // This method should be called as an animation event at the end of the attack animation.
    public void AttackFinished()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        Vector3 pos = transform.position;
        if (animator.GetFloat("lastXVel") < 0)
            {
                pos.x += attackOffsetX; // Slight position adjustment to avoid clipping issues
                StartCoroutine(WaitAndUpdatePosition(pos));
            }
            else if (animator.GetFloat("lastXVel") > 0)
            {
                pos.x -= attackOffsetX; // Slight position adjustment to avoid clipping issues
                StartCoroutine(WaitAndUpdatePosition(pos));
            }
    }
}
