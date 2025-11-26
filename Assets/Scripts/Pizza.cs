using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Pizza : MonoBehaviour
{
    public float damage = 6f;
    public float knockback = 2f;

    public GameObject pepperoni;
    public GameObject cheese;
    public GameObject spicy;

    public GameObject splashEffect;

    private PlayerStates playerStates;
    private List<IDamageable> hitTargets = new List<IDamageable>();

    private bool applySlow = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStates = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerStates>();


        if (playerStates.powerUps.Contains("pepperoni"))
        {
            pepperoni.SetActive(true);
        }
        
        if (playerStates.powerUps.Contains("cheese"))
        {
            cheese.SetActive(true);
            applySlow = true;
        }

        if (playerStates.powerUps.Contains("spicy"))
        {
            spicy.SetActive(true);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null && !collision.CompareTag("PlayerHurtbox") && !hitTargets.Contains(damageable))
        {
            // PASS THE KNOCKBACK HERE
            damageable.TakeDamage(damage, knockback, applySlow); 
            hitTargets.Add(damageable);

            destroyPizza();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            destroyPizza();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "bulletRange")
        {
            destroyPizza();
        }
    }

    private void destroyPizza()
    {
        GameObject splashEffectInstantiated = Instantiate(splashEffect);
        splashEffectInstantiated.transform.position = transform.position;

        Destroy(gameObject);
    }
}
