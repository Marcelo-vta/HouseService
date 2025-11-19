using UnityEngine;

/// <summary>
/// Base class for all enemies. Manages health, stats, and dying.
/// It also implements IDamageable so it can be damaged by other sources.
/// </summary>
public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyStats enemyStats;

    private float currentHealth;

    private void Start()
    {
        if (enemyStats != null)
        {
            currentHealth = enemyStats.maxHealth;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Add death logic here, like playing an animation and destroying the object
        Destroy(gameObject);
    }

    /// <summary>
    /// Called by Unity's physics engine when this object's collider touches another.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Try to find a component on the object we hit that can be damaged.
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        // If the object is damageable and it's not ourselves...
        if (damageable != null && collision.gameObject != this.gameObject)
        {
            // ...deal damage to it.
            damageable.TakeDamage(enemyStats.damage);
        }
    }
}
