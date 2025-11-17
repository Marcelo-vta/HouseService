using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Enemies/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public float maxHealth = 100f;
    public float speed = 3f;
    
    [Header("Combat")]
    public float damage = 10f;
    public float attackLungeSpeed = 6f;
}
