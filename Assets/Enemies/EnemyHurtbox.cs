using UnityEngine;

public class EnemyHurtbox : MonoBehaviour, IDamageable
{
    [SerializeField] Enemy mainEnemyScript;
    [SerializeField] Boss boss;

    // This function is called when the Player's Slash hits this specific collider
    public void TakeDamage(float damageAmount, float knockbackForce = 0f)
    {
        if (mainEnemyScript != null)
        {
            // Pass the damage up to the main script on the parent
            mainEnemyScript.TakeDamage(damageAmount, knockbackForce);
        }
        if (boss != null)
        {
            // Pass the damage up to the main script on the parent
            boss.TakeDamage(damageAmount, knockbackForce);
        }
    }
}