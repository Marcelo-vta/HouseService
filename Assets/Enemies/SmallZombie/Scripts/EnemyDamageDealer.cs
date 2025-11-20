using System.Data.Common;
using UnityEditor;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    private MeleeEnemyStats meleeStats;
    [SerializeField]
    public float customDamage; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if we hit the Player
        if (collision.CompareTag("Player"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                if (customDamage != 0){
                    damageable.TakeDamage(customDamage);
                    return;
                }

                damageable.TakeDamage(meleeStats.damage);
            }
        }
    }
}