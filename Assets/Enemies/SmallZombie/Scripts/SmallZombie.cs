using UnityEngine;
using UnityEngine.AI;

public class SmallZombie : Enemy
{
    [Header("Melee Components")]
    [SerializeField] GameObject hitbox; 
    

    private MeleeEnemyStats meleeStats;
    private bool isLeaping = false;
    private float leapTimer = 0f;
    private float nextAttackTime = 0f;
    private Vector3 fixedLeapDirection; 
    
    private Animator animator;
    
    // New variable to store the duration found in the animation
    private float actualLeapDuration;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        meleeStats = stats as MeleeEnemyStats;
        if (meleeStats == null)
        {
            Debug.LogError("Error: Wrong Stats assigned to Small Zombie!");
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
            HandleLeap();
            // When leaping, our move direction is the locked trajectory
            moveDirection = fixedLeapDirection;
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
    }

    private void HandleChase()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (agent.enabled) 
        {
            agent.SetDestination(target.position);
            agent.isStopped = false;
        }

        if (distance <= meleeStats.attackRange && Time.time >= nextAttackTime)
        {
            StartLeap();
        }
    }

    private void StartLeap()
    {
        isLeaping = true;
        leapTimer = 0f;

        // 1. Trigger Animation
        if (animator != null)
        {
            animator.SetBool("isLongAttacking", true);
            
            // 2. Force the Animator to update its state machine immediately
            animator.Update(0f);

            // 3. Get the duration of the state we are transitioning TO
            AnimatorStateInfo info = animator.GetNextAnimatorStateInfo(0);
            
            // If the transition is instant (no duration), GetNext might be empty, so check GetCurrent
            if (info.fullPathHash == 0) 
            {
                info = animator.GetCurrentAnimatorStateInfo(0);
            }

            // 4. Set the leap duration based on the animation length
            if (info.length > 0)
            {
                actualLeapDuration = info.length;
            }
        }

        fixedLeapDirection = (target.position - transform.position).normalized;

        agent.isStopped = true; 
        agent.ResetPath();

        if (hitbox != null) hitbox.SetActive(true);
    }

    private void HandleLeap()
    {
        leapTimer += Time.deltaTime;

        // USE actualLeapDuration HERE
        float t = leapTimer / actualLeapDuration;

        float decay = Mathf.Pow(1 - t, 3) / (t + 0.01f);
        
        float currentSpeed = Mathf.Lerp(meleeStats.moveSpeed, meleeStats.leapSpeed, decay);

        transform.position += fixedLeapDirection * currentSpeed * Time.deltaTime;

        // Check against actualLeapDuration
        if (leapTimer >= actualLeapDuration)
        {
            EndLeap();
        }
    }

    private void EndLeap()
    {
        isLeaping = false;

        if (animator != null) animator.SetBool("isLongAttacking", false);
        
        agent.isStopped = false;

        if (hitbox != null) hitbox.SetActive(false);

        nextAttackTime = Time.time + meleeStats.attackCooldown;
    }
}