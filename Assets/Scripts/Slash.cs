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
    public float damage = 2f; 
    public float knockback = 5f; // This value will now be sent to the enemy

    private float baseDamage;
    private float baseKnockback;
    private Vector3 baseScale;

    private float timer = 0f;
    private PlayerStates playerStates;
    private List<IDamageable> hitTargets = new List<IDamageable>();

    private bool appliesSlow;

    void Start()
    {
        playerStates = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStates>();
        activeSprites = defaultSprites;

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        int flipVertical = rotationZ < -90 || rotationZ > 90 ? -1 : 1;
        
        Vector3 currentScale = transform.localScale;
        currentScale.y = Mathf.Abs(currentScale.y) * flipVertical; 
        transform.localScale = currentScale;

        baseScale = transform.localScale;

        if (playerStates.powerUps.Contains("mop"))
        {
            damage *= 1.5f; 
        }

        if (playerStates.powerUps.Contains("witch"))
        {
            playerStates.attackSpeed = 50;
        }

        if (playerStates.powerUps.Contains("long"))
        {
            knockback *= 1.5f;
            currentScale *= 1.7f;
        }

        if (playerStates.powerUps.Contains("wet"))
        {
            activeSprites = wetSprites;
            appliesSlow = true;
        }

        transform.localScale = currentScale;

    }

    void Update()
    {
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


        if (collision.CompareTag("EnemyHurtbox"))
        {
            // PASS THE KNOCKBACK HERE
            damageable.TakeDamage(damage, knockback, appliesSlow);            
            hitTargets.Add(damageable);
        }
    }
}