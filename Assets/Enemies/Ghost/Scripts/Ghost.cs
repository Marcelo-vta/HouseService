using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GhostEnemy : Enemy
{
    [Header("Melee Components")]
    [SerializeField] GameObject hitbox; 
    

    private MeleeEnemyStats meleeStats;
    private bool isLeaping = false;
    private float leapTimer = 0f;
    private float nextAttackTime = 0f;
    private Vector3 fixedLeapDirection;
    private bool canLeap = true;

    public float leapSpeed;
    public float leapCooldown;
    
    private Animator animator;
    private Rigidbody2D rb;
    
    // New variable to store the duration found in the animation
    private float actualLeapDuration;
    private bool leapFlip = false;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (animator == null) animator = GetComponentInChildren<Animator>();

        meleeStats = stats as MeleeEnemyStats;
        if (meleeStats == null)
        {
            Debug.LogError("Error: Wrong Stats assigned to Ghost!");
        }

        // Default to stats value first (fallback)
        actualLeapDuration = meleeStats != null ? meleeStats.leapDuration : 0.5f;

        // We no longer loop through clips here. We get the duration dynamically in StartLeap.

        if (hitbox != null) hitbox.SetActive(false);
    }

    private void Update()
    {
        if (target == null || meleeStats == null) return;

        Vector3 moveDirection = Vector3.zero;

        // STATE MACHINE
        if (isLeaping)
        {
            moveDirection = rb.linearVelocity;
        }
        else
        {
            HandleChase();
            // When chasing, the NavMeshAgent handles the physics
            moveDirection = agent.velocity;
        }

        // --- ANIMATION MOVEMENT UPDATE ---
        // Only update the direction if we are actually moving.
        // This keeps the "Last" direction stored when we stop.
        if (animator != null && moveDirection.sqrMagnitude > 0.1f)
        {
            moveDirection.Normalize(); // Get pure direction (Length of 1)
            animator.SetFloat("lastXVel", moveDirection.x);
            animator.SetFloat("lastYVel", moveDirection.y);
        }

        animator.SetFloat("yVel", moveDirection.y);
        animator.SetFloat("xVel", moveDirection.x);

        animator.SetFloat("Vel", moveDirection.magnitude);

        animator.gameObject.GetComponent<SpriteRenderer>().flipX = moveDirection.x < 0 || leapFlip;
    }

    private void HandleChase()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (agent.enabled) 
        {
            agent.SetDestination(target.position);
            agent.isStopped = false;
        }

        if (distance <= meleeStats.attackRange && canLeap)
        {
            StartCoroutine(LeapCoroutine());
        }
    }


    IEnumerator LeapCoroutine()
    {
        canLeap = false;
        isLeaping = true;

        animator.SetBool("isAttacking", true);
        animator.Update(0f);

        agent.isStopped = true; 
        agent.ResetPath();
        leapFlip = (target.transform.position - transform.position).normalized.x < 0;

        yield return new WaitForSeconds(.45f);
        float leapDuration = .45f;

        if (hitbox != null) hitbox.SetActive(true);

        Vector2 dashDirection = (target.transform.position - transform.position).normalized;
        float distanceMagnitude = (target.transform.position - transform.position).magnitude;


        rb.linearVelocity = dashDirection * leapSpeed;
        yield return new WaitForSeconds(leapDuration - (leapDuration / 3));

        rb.linearVelocity = dashDirection * leapSpeed / 2;
        yield return new WaitForSeconds(leapDuration / 3 - (leapDuration / 5));

        rb.linearVelocity = dashDirection * leapSpeed / 4;
        yield return new WaitForSeconds(leapDuration / 5);

        if (hitbox != null) hitbox.SetActive(false);

        rb.linearVelocity = Vector2.zero; // Stop the dash movement
        animator.SetBool("isAttacking", false);
        agent.isStopped = false;
        isLeaping = false;
        leapFlip = false;

        yield return new WaitForSeconds(leapCooldown);
        canLeap = true;
    }
}