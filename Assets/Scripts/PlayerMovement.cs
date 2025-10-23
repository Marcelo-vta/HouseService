using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;

    public Animator animator;

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
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");


        Vector2 moveInput = new Vector2(moveHorizontal, moveVertical);
        Vector2 movement = moveInput.normalized * speed * Time.fixedDeltaTime;


        animator.SetFloat("speed", (float)Math.Pow(movement.magnitude, 2));
        rb.MovePosition(rb.position + movement);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Roll();    
        }
        
    }

    public void Roll()
    {
        speed *= 2;
    }

}
