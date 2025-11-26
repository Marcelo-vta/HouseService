using System.Collections.Generic;
using UnityEngine;

public class Pepperoni : MonoBehaviour
{
    public float damage = 6f;
    public float knockback = 2f;

    private List<IDamageable> hitTargets = new List<IDamageable>();


    void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null && !collision.CompareTag("PlayerHurtbox") && !hitTargets.Contains(damageable))
        {
            // PASS THE KNOCKBACK HERE
            damageable.TakeDamage(damage, knockback); 
            hitTargets.Add(damageable);

            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "bulletRange")
        {
            Destroy(gameObject);
        }
    }
}
