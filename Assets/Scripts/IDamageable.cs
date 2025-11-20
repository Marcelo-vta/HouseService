using UnityEngine;

public interface IDamageable
{
    // We add 'knockback' with a default value of 0 
    // so we don't break scripts that haven't been updated yet.
    void TakeDamage(float damageAmount, float knockbackForce = 0f);
}