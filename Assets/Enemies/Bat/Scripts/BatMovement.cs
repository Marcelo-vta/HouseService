using UnityEngine;

public class BatMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    public float speed;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 movement;
    private Vector2 movementBase;


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
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 moveInput = new Vector2(moveHorizontal, moveVertical);

        movementBase = moveInput.normalized * Time.fixedDeltaTime;

        if (movementBase.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementBase.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        movement = movementBase * speed;
        rb.MovePosition(rb.position + movement);
    }
}
