using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float lifetime = 5f;

    private float damage; // Set by the enemy

    private void Start()
    {
        // Destroy the bullet after X seconds so it doesn't exist forever
        Destroy(gameObject, lifetime);
    }

    // NOTE: Update() is removed because Rigidbody2D handles the movement now.

    public void SetDamage(float damageAmount)
    {
        this.damage = damageAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Did we hit the Player?
        IDamageable damageable = collision.GetComponent<IDamageable>();
        
        if (collision.CompareTag("PlayerHurtbox"))
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject); // Bullet disappears on impact
            return;
        }

        // 2. Did we hit a Wall?
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            Destroy(gameObject);
        }
    }
}