using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the Bat's ranged attack by instantiating a projectile.
/// Implements the IAttack interface to be compatible with our generic AI scripts.
/// </summary>
public class BatAttack : MonoBehaviour, IAttack
{
    [Header("Attack Setup")]
    [Tooltip("The projectile prefab to be fired.")]
    public GameObject projectilePrefab;
    [Tooltip("The point from which the projectile is fired.")]
    public Transform firePoint;

    [Header("Animation")]
    [Tooltip("The exact name of the attack animation state in the Animator.")]
    public string attackStateName = "Attack";

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float attackDuration = 0.5f; // Default fallback duration

    // Component references
    private Enemy enemy;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();

        if (animator == null) Debug.LogError("Animator component not found!", this);
        if (enemy == null || enemy.enemyStats == null)
        {
            Debug.LogError("Enemy or EnemyStats not found! Disabling attack script.", this);
            this.enabled = false;
            return;
        }
        if (projectilePrefab == null) Debug.LogError("Projectile Prefab is not assigned!", this);
        if (firePoint == null)
        {
            firePoint = this.transform;
        }
    }

    void Update()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDuration)
            {
                isAttacking = false;
            }
        }
    }

    public void Attack(Transform target)
    {
        if (isAttacking || animator == null) return;

        isAttacking = true;
        attackTimer = 0f;

        // Start the animation and get its length
        animator.CrossFade(attackStateName, 0.1f, 0);
        
        var clips = animator.GetCurrentAnimatorClipInfo(0);
        if (clips.Length > 0)
        {
            attackDuration = clips[0].clip.length;
        }

        // Fire the projectile, aimed at the target
        FireProjectile(target);
    }

    private void FireProjectile(Transform target)
    {
        if (projectilePrefab == null) return;

        Transform spawnPoint = (firePoint != null) ? firePoint : this.transform;
        
        // Calculate rotation to look at the player
        Vector2 direction = (target.position - spawnPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Instantiate the projectile with the correct rotation
        Instantiate(projectilePrefab, spawnPoint.position, rotation);
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    // For a ranged attacker, GetCurrentSpeed should just be the base speed,
    // as it doesn't lunge.
    public float GetCurrentSpeed()
    {
        if (enemy != null && enemy.enemyStats != null)
        {
            return enemy.enemyStats.speed;
        }
        return 0f;
    }
}
