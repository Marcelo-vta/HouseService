using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Enemy/Base Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("General Stats")]
    public string enemyName;
    public float maxHealth = 100f;
    public float moveSpeed = 3.5f;
    public int damage = 10;

    [Range(0,1)]
    public float weight = 1;
}