using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] defaultSprites;
    public Sprite[] wetSprites;
    private Sprite[] activeSprites;

    [Header("Stats")]
    public float damage = 10f; 
    public float knockback = 5f; // This value will now be sent to the enemy

    private float baseDamage;
    private float baseKnockback;
    private Vector3 baseScale;

    private float timer = 0f;
    private PlayerStates playerStates;
    private List<IDamageable> hitTargets = new List<IDamageable>();

    void Start()
    {
        playerStates = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStates>();
        activeSprites = defaultSprites;

        baseDamage = damage;
        baseKnockback = knockback;

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        int flipVertical = rotationZ < -90 || rotationZ > 90 ? -1 : 1;
        
        Vector3 currentScale = transform.localScale;
        currentScale.y = Mathf.Abs(currentScale.y) * flipVertical; 
        transform.localScale = currentScale;

        baseScale = transform.localScale;
    }

    void Update()
    {
        damage = baseDamage;
        knockback = baseKnockback;
        Vector3 targetScale = baseScale;
        activeSprites = defaultSprites;

        if (playerStates.powerUps.Contains("mop"))
        {
            damage *= 1.5f; 
        }

        if (playerStates.powerUps.Contains("witch"))
        {
            knockback *= 1.5f;
        }

        if (playerStates.powerUps.Contains("long"))
        {
            targetScale *= 1.2f;
        }

        if (playerStates.powerUps.Contains("wet"))
        {
            activeSprites = wetSprites;
        }

        transform.localScale = targetScale;


        timer += Time.deltaTime;

        if (timer < .15f)
        {
            spriteRenderer.sprite = activeSprites[0];
        }
        else
        {
            spriteRenderer.sprite = activeSprites[1];
        }

        if (timer >= .3f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log($"[Projectile] Hit: {collision.gameObject.name} | Layer: {LayerMask.LayerToName(collision.gameObject.layer)}");
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null && !collision.CompareTag("Player") && !hitTargets.Contains(damageable))
        {
            // PASS THE KNOCKBACK HERE
            damageable.TakeDamage(damage, knockback); 
            
            hitTargets.Add(damageable);
        }
    }
}