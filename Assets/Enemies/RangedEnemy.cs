using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : Enemy
{
    [Header("Scene Specifics")]
    [SerializeField] LayerMask obstacleLayer; 

    private const float innacuracyConstant = 15f; 
    private RangedEnemyStats rangedStats;
    private float nextAttackTime = 0f;

    protected override void Start()
    {
        base.Start();
        rangedStats = stats as RangedEnemyStats;

        if (rangedStats == null)
        {
            Debug.LogError("Error: Assigned Basic Stats to Ranged Enemy.");
        }
    }

    private void Update()
    {
        if (target == null || rangedStats == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        bool canSeePlayer = HasLineOfSight();

        // --- 1. MOVEMENT LOGIC ---
        // Handle positioning separately from shooting
        if (!canSeePlayer)
        {
            // If blocked, chase to find the player
            agent.SetDestination(target.position);
        }
        else
        {
            // If we see the player, manage distance
            if (distanceToTarget < rangedStats.retreatDistance)
            {
                RetreatFromPlayer();
            }
            else if (distanceToTarget > rangedStats.stoppingDistance)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                // Perfect Range? Stop.
                agent.ResetPath();
            }
        }

        // --- 2. ATTACK LOGIC ---
        // This is now separate. As long as we have LOS, we shoot.
        if (canSeePlayer && Time.time >= nextAttackTime)
        {
            Shoot();
            nextAttackTime = Time.time + (1f / rangedStats.fireRate); 
        }
    }

    private void Shoot()
    {
        if (target == null) return;

        // 1. Perfect Aim
        Vector3 directionToTarget = target.position - transform.position;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        Quaternion perfectRotation = Quaternion.Euler(0, 0, angle);

        // 2. Inaccuracy
        float inaccuracyFactor = 1f - rangedStats.shotAccuracy;
        float currentInaccuracy = Random.Range(-inaccuracyFactor, inaccuracyFactor);
        float shotOffset = currentInaccuracy * innacuracyConstant;

        // 3. Final Rotation
        Quaternion offsetRotation = Quaternion.Euler(0, 0, shotOffset);
        Quaternion finalRotation = perfectRotation * offsetRotation;

        // 4. Instantiate
        GameObject bullet = Instantiate(rangedStats.projectilePrefab, transform.position, finalRotation);

        // 5. Setup Bullet
        EnemyProjectile projScript = bullet.GetComponent<EnemyProjectile>();
        if (projScript != null) 
        {
            projScript.SetDamage(rangedStats.damage);
        }

        // 6. Apply Force
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(bullet.transform.right * rangedStats.projectileForce, ForceMode2D.Impulse);
        }
    }

    private bool HasLineOfSight()
    {
        Vector2 direction = target.position - transform.position;
        float distance = direction.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleLayer);
        return hit.collider == null; 
    }

    private void RetreatFromPlayer()
    {
        Vector3 dirToPlayer = (transform.position - target.position).normalized;

        if (TryMoveIdeally(dirToPlayer)) return;
        if (TryMoveIdeally(Quaternion.Euler(0, 0, 45) * dirToPlayer)) return;
        if (TryMoveIdeally(Quaternion.Euler(0, 0, -45) * dirToPlayer)) return;
    }

    private bool TryMoveIdeally(Vector3 direction)
    {
        Vector3 targetPos = transform.position + direction * rangedStats.fleeDistance;
        
        NavMeshHit hit;
        bool blocked = NavMesh.Raycast(transform.position, targetPos, out hit, NavMesh.AllAreas);

        if (!blocked)
        {
            agent.SetDestination(targetPos);
            return true;
        }
        return false;
    }
}