using UnityEngine;
using UnityEngine.AI;

public class GhostEnemy : Enemy
{
    [Header("Melee Components")]
    [SerializeField] GameObject hitbox; 

    private MeleeEnemyStats meleeStats;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float nextAttackTime = 0f;
    
    private Animator animator;
    private float actualAttackDuration;

    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();
        if (animator == null) animator = GetComponentInChildren<Animator>();

        meleeStats = stats as MeleeEnemyStats;
        if (meleeStats == null)
        {
            Debug.LogError("Error: Wrong Stats assigned to Ghost! Please assign MeleeStats.");
        }

        // Use leapDuration from stats as a fallback for attack duration
        actualAttackDuration = meleeStats != null ? meleeStats.leapDuration : 0.5f;

        if (hitbox != null) hitbox.SetActive(false);
    }

    private void Update()
    {
        if (target == null || meleeStats == null) return;

        // --- 1. MOVEMENT (Always Chase) ---
        // Unlike the SmallZombie, we NEVER stop the agent.
        if (agent.enabled)
        {
            agent.SetDestination(target.position);
            agent.isStopped = false; 
        }

        // --- 2. ATTACK LOGIC ---
        if (isAttacking)
        {
            HandleAttackTimer();
        }
        else
        {
            CheckForAttack();
        }

        // --- 3. ANIMATION & FLIP ---
        HandleAnimation();
    }

    private void CheckForAttack()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        // If in range and cooldown is ready -> Attack!
        if (distance <= meleeStats.attackRange && Time.time >= nextAttackTime)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        attackTimer = 0f;

        // Enable Hitbox
        if (hitbox != null) hitbox.SetActive(true);

        // Trigger Animation & Get Duration
        if (animator != null)
        {
            animator.SetBool("isAttacking", true);
            animator.Update(0f);
            
            AnimatorStateInfo info = animator.GetNextAnimatorStateInfo(0);
            if (info.fullPathHash == 0) 
            {
                info = animator.GetCurrentAnimatorStateInfo(0);
            }

            if (info.length > 0)
            {
                actualAttackDuration = info.length;
            }
        }
    }

    private void HandleAttackTimer()
    {
        attackTimer += Time.deltaTime;

        // Stop attacking after animation finishes
        if (attackTimer >= actualAttackDuration)
        {
            EndAttack();
        }
    }

    private void EndAttack()
    {
        isAttacking = false;

        // Disable Hitbox
        if (hitbox != null) hitbox.SetActive(false);
        
        // Stop Animation
        if (animator != null) animator.SetBool("isAttacking", false);

        // Set Cooldown
        nextAttackTime = Time.time + meleeStats.attackCooldown;
    }

    private void HandleAnimation()
    {
        // Use Agent velocity for animation since we are always moving via NavMesh
        Vector3 moveDirection = agent.velocity;

        if (animator != null && moveDirection.sqrMagnitude > 0.1f)
        {
            moveDirection.Normalize();
            animator.SetFloat("lastXVel", moveDirection.x);
            animator.SetFloat("lastYVel", moveDirection.y);

            // Flip Sprite
            Vector3 currentScale = transform.localScale;
            if (moveDirection.x < -0.01f)
            {
                currentScale.x = -Mathf.Abs(currentScale.x);
            }
            else if (moveDirection.x > 0.01f)
            {
                currentScale.x = Mathf.Abs(currentScale.x);
            }
            transform.localScale = currentScale;
        }
    }
}