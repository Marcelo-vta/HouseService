using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedStats", menuName = "Enemy/Ranged Stats")]
public class RangedEnemyStats : EnemyStats
{
    [Header("Ranged AI Settings")]
    public float stoppingDistance = 5f;
    public float retreatDistance = 3f;
    public float fleeDistance = 4f;

    [Header("Combat")]
    public GameObject projectilePrefab;
    public float fireRate = 1f;

    [Header("Ballistics")]
    public float projectileForce = 10f; // Replaces 'speed'
    [Range(0f, 1f)] public float shotAccuracy = 0.9f; // 1.0 is perfect aim
}