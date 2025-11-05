using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    public Animator animator;
    private bool hurt;

    private float currentTime = 0f;

    public float ivulnerableTime = .7f;
    public float recoverTime = .5f;

    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider2D;
    public PlayerRoll playerRoll;

    public GameObject hands;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            hit();
        }

        if (hurt)
        {
            currentTime += Time.deltaTime;

            if (currentTime > ivulnerableTime)
            {
                hurt = false;
                setActiveHurtbox(true);
            }
        }
    }

    void hit()
    {
        animator.SetTrigger("hurt");
        hurt = true;

        currentTime = 0;

        setActiveHurtbox(false);
    }
    
    void setActiveHurtbox(bool value)
    {
        spriteRenderer.enabled = value;
        capsuleCollider2D.enabled = value;
        playerRoll.enabled = value;
        hands.SetActive(value);
        
    }
}
