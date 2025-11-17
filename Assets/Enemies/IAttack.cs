using UnityEngine;

/// <summary>
/// Defines a contract for any enemy attack script.
/// This allows a generic AI to communicate with a specific attack behavior
/// without needing to know its concrete type.
/// </summary>
public interface IAttack
{
    /// <summary>
    /// Initiates the attack sequence, aimed at a specific target.
    /// </summary>
    /// <param name="target">The transform of the target to aim at.</param>
    void Attack(Transform target);

    /// <summary>
    /// Returns true if the attack sequence is currently active.
    /// </summary>
    bool IsAttacking();

    /// <summary>
    /// Gets the current movement speed, which may be modified by the attack (e.g., a lunge).
    /// </summary>
    float GetCurrentSpeed();
}
