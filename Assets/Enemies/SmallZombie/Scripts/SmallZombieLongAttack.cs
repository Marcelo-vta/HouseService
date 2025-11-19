using UnityEngine;
using System.Collections;

/// <summary>
/// This script manages the state and speed of the zombie's lunge attack,
/// behaving similarly to the Player's Roll script.
/// </summary>
public class SmallZombieLongAttack : MonoBehaviour, IAttack
{
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float attackDuration = 0f;

    // Speed variables
    private float baseSpeed;
    private float lungeSpeed;
    private float currentSpeed;

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

        // Initialize speeds from stats
        baseSpeed = enemy.enemyStats.speed;
        lungeSpeed = enemy.enemyStats.attackLungeSpeed;
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            // Use a decay curve similar to PlayerRoll
            float t = attackTimer / attackDuration;
            float decay = Mathf.Pow(1 - t, 2); // Using a simpler squared curve for the lunge
            currentSpeed = Mathf.Lerp(baseSpeed, lungeSpeed, decay);

            // Stop attacking when done
            if (attackTimer >= attackDuration)
            {
                isAttacking = false;
                currentSpeed = baseSpeed;
            }
        }
        
        if (animator != null)
        {
            animator.SetBool("isLongAttacking", isAttacking);
        }
    }

    public void Attack(Transform target) // Target parameter is unused for melee, but required by IAttack
    {
        if (isAttacking || animator == null) return;

        isAttacking = true;
        attackTimer = 0f;
        StartCoroutine(SetAttackDuration());
    }

    private IEnumerator SetAttackDuration()
    {
        yield return null; // Wait a frame for the animator state to update
        attackDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        if (attackDuration <= 0) attackDuration = 0.5f; // Fallback
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    /// <summary>
    /// Called by the AI script every frame to get the correct movement speed.
    /// </summary>
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
