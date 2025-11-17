using UnityEngine;

/// <summary>
/// Sits on a child object and acts as a receiver for damage,
/// passing it to any IDamageable component on its parent.
/// </summary>
public class EnemyHurtbox : MonoBehaviour
{
    private IDamageable damageable;

    private void Start()
    {
        damageable = GetComponentInParent<IDamageable>();
    }

    // This method can be called by player weapons or other damage sources.
    public void TakeDamage(float damage)
    {
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
}
