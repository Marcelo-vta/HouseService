using UnityEngine;
using Pathfinding;

/// <summary>
/// A generic AI for ranged enemies. It tries to maintain an ideal distance
/// from its target, backing away if too close and advancing if too far.
/// It only attacks when within its ideal range.
/// </summary>
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Enemy))]
public class RangeEnemyAI : MonoBehaviour
{
    [Header("AI Logic")]
    public Transform target;
    public float nextWaypointDistance = 3f;
    public float pathUpdateInterval = 0.5f;

    [Header("Ranged Behavior")]
    [Tooltip("The ideal distance to keep from the player.")]
    public float idealRange = 7f;
    [Tooltip("The distance at which the enemy will start backing away.")]
    public float tooCloseRange = 4f;

    [Header("Attack")]
    public float attackCooldown = 3f;
    private float lastAttackTime;

    // Internal state
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    // Component references
    private Seeker seeker;
    private Rigidbody2D rb;
    private Enemy enemy;
    private IAttack enemyAttack;
    private Animator animator;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        enemyAttack = GetComponent<IAttack>();
        animator = GetComponent<Animator>();

        if (enemy.enemyStats == null) Debug.LogError("EnemyStats not assigned!", this);
        if (enemyAttack == null) Debug.LogError("No IAttack component found!", this);

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }

        if (target != null) InvokeRepeating("UpdatePath", 0f, pathUpdateInterval);
        lastAttackTime = -999f;
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null || enemy.enemyStats == null || enemyAttack == null || path == null)
        {
            if (animator != null) animator.SetFloat("Vel", 0);
            return;
        }

        float distanceToTarget = Vector2.Distance(rb.position, target.position);
        Vector2 directionToTarget = ((Vector2)target.position - rb.position).normalized;

        // --- MOVEMENT LOGIC ---
        bool shouldMove = true;

        // If we are in the ideal range, don't move, just attack.
        if (distanceToTarget <= idealRange && distanceToTarget > tooCloseRange)
        {
            shouldMove = false;
        }

        if (enemyAttack.IsAttacking())
        {
            shouldMove = false; // Also stop moving while the attack animation plays
        }

        if (shouldMove)
        {
            // If we are too close, the direction should be away from the player.
            Vector2 moveDirection = (distanceToTarget < tooCloseRange) ? -directionToTarget : ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            
            float currentSpeed = enemy.enemyStats.speed;
            Vector2 velocity = moveDirection * currentSpeed;
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            UpdateAnimation(velocity);

            if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
            {
                currentWaypoint++;
            }
        }
        else
        {
            // Not moving, so velocity is zero.
            UpdateAnimation(Vector2.zero);
        }

        // --- ATTACK LOGIC ---
        if (!enemyAttack.IsAttacking() && Time.time > lastAttackTime + attackCooldown)
        {
            // Only attack if we are in the sweet spot.
            if (distanceToTarget <= idealRange && distanceToTarget > tooCloseRange)
            {
                // Face the player before attacking
                UpdateAnimation(directionToTarget);
                enemyAttack.Attack(target);
                lastAttackTime = Time.time;
            }
        }
    }

    private void UpdateAnimation(Vector2 velocity)
    {
        if (animator == null) return;

        if (velocity.x < -0.01f) transform.localScale = new Vector3(-1, 1, 1);
        else if (velocity.x > 0.01f) transform.localScale = new Vector3(1, 1, 1);

        animator.SetFloat("Vel", velocity.magnitude);
        Vector2 normalized = velocity.normalized;
        animator.SetFloat("xVel", normalized.x);
        animator.SetFloat("yVel", normalized.y);

        if (velocity.sqrMagnitude > 0.01f)
        {
            animator.SetFloat("lastXVel", normalized.x);
            animator.SetFloat("lastYVel", normalized.y);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, idealRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, tooCloseRange);
    }
}
