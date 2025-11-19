/// <summary>
/// Defines a contract for any object that can take damage.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Causes the object to take a specified amount of damage.
    /// </summary>
    /// <param name="damageAmount">The amount of damage to inflict.</param>
    void TakeDamage(float damageAmount);
}