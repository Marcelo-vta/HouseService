using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    private MeleeEnemyStats meleeStats;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if we hit the Player
        if (collision.CompareTag("Player"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(meleeStats.damage);
            }
        }
    }
}