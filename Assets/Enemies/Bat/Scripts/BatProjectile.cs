using UnityEngine;

/// <summary>
/// Controls the behavior of the projectile fired by the Bat.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BatProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f;
    public float lifetime = 5f; // Time in seconds before the projectile is destroyed
    public float damage = 5f; // Damage to deal on impact

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Projectiles shouldn't be affected by gravity

        // Propel the projectile forward
        rb.linearVelocity = transform.right * speed;

        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Try to find a component on the object we hit that can be damaged.
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        // If the object is damageable...
        if (damageable != null)
        {
            // ...and it's not an enemy...
            if (collision.gameObject.GetComponent<Enemy>() == null)
            {
                // ...deal damage to it and destroy the projectile.
                damageable.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            // If the object we hit isn't damageable (like a wall),
            // and it's not another enemy, destroy the projectile.
            if (collision.gameObject.GetComponent<Enemy>() == null)
            {
                 Destroy(gameObject);
            }
        }
    }
}
