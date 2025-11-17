using UnityEngine;
using Pathfinding;

/// <summary>
/// A generic AI for melee enemies. It handles pathfinding, movement, and triggering attacks.
/// It communicates with a specific attack behavior via the IAttack interface,
/// allowing it to be used for any enemy that has a component that implements IAttack.
/// </summary>
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Enemy))]
public class MeleeEnemyAI : MonoBehaviour
{
    [Header("AI Logic")]
    public Transform target;
    public float nextWaypointDistance = 3f;
    public float pathUpdateInterval = 0.5f;

    [Header("Attack")]
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    // Internal state
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    // Component references
    private Seeker seeker;
    private Rigidbody2D rb;
    private Enemy enemy;
    private IAttack enemyAttack; // The generic attack interface
    private Animator animator;

    void Start()
    {
        // --- Get Components ---
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        enemyAttack = GetComponent<IAttack>(); // Finds any component that implements IAttack
        animator = GetComponent<Animator>();

        // --- Validate Components ---
        if (animator == null) Debug.LogError("Animator component not found on " + gameObject.name, this);
        if (enemy.enemyStats == null) Debug.LogError("EnemyStats asset not assigned in the Enemy component on " + gameObject.name, this);
        if (enemyAttack == null) Debug.LogError("No component implementing IAttack found on " + gameObject.name, this);

        // --- Targetting ---
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogWarning("AI has no target and 'Player' tag not found.", this);
            }
        }

        // --- Pathfinding ---
        if (target != null)
        {
            InvokeRepeating("UpdatePath", 0f, pathUpdateInterval);
        }

        // Initialize with a large negative value to ensure the first attack is immediate
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
        else
        {
            Debug.LogWarning("Pathfinding error: " + p.errorLog, this);
        }
    }

    void FixedUpdate()
    {
        // Stop everything if critical components are missing or we have no target
        if (target == null || enemy.enemyStats == null || enemyAttack == null || path == null)
        {
            if (animator != null) animator.SetFloat("Vel", 0);
            return;
        }

        // Check if we are at the end of the path
        reachedEndOfPath = currentWaypoint >= path.vectorPath.Count;
        if (reachedEndOfPath)
        {
            if (animator != null) animator.SetFloat("Vel", 0);
            return;
        }

        // --- Get Speed from Attack Script ---
        float currentSpeed = enemyAttack.GetCurrentSpeed();

        // --- Movement ---
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 velocity = direction * currentSpeed;
        Vector2 newPosition = rb.position + velocity * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
        UpdateAnimation(velocity);

        // --- Waypoint Progress ---
        float distanceToWaypoint = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distanceToWaypoint < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // --- Attack Trigger Logic ---
        if (!enemyAttack.IsAttacking())
        {
            float distanceToTarget = Vector2.Distance(rb.position, target.position);
            if (distanceToTarget <= attackRange && Time.time > lastAttackTime + attackCooldown)
            {
                enemyAttack.Attack(target);
                lastAttackTime = Time.time;
            }
        }
    }

    private void UpdateAnimation(Vector2 velocity)
    {
        if (animator == null) return;

        // Flip sprite based on horizontal velocity
        if (velocity.x < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (velocity.x > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Update animator parameters
        animator.SetFloat("Vel", velocity.magnitude);

        Vector2 normalizedVelocity = velocity.normalized;
        animator.SetFloat("xVel", normalizedVelocity.x);
        animator.SetFloat("yVel", normalizedVelocity.y);

        if (velocity.sqrMagnitude > 0.01f)
        {
            animator.SetFloat("lastXVel", normalizedVelocity.x);
            animator.SetFloat("lastYVel", normalizedVelocity.y);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
