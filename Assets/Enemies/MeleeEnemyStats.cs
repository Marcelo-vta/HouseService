using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeStats", menuName = "Enemy/Melee Stats")]
public class MeleeEnemyStats : EnemyStats
{
    [Header("Leap Attack Settings")]
    public float attackRange = 3f;       // Distance to start jumping
    public float leapDuration = 0.5f;    // How long the jump lasts
    public float leapSpeed = 20f;        // Max speed during the jump (Speed Boost)
    public float attackCooldown = 2f;    // Time between jumps
}