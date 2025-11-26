using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Base Settings")]
    public EnemyStats stats;

    [Header("Visual Feedback")]
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = 0.1f;

    // We need to verify if the target (Player) is assigned to calculate knockback direction
    [SerializeField] protected Transform target;

    protected NavMeshAgent agent;
    public float currentHealth;

    public GameObject deathEffect;

    // Visuals
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine flashRoutine;
    private Coroutine knockbackRoutine; // Track knockback to avoid overlap

    private float moveSpeed;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip hitClip;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.material;
        }

        // 2D NavMesh Configuration
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (stats != null)
        {
            currentHealth = stats.maxHealth;
            agent.speed = stats.moveSpeed;
        }

        // Logic to find player if not assigned
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) target = playerObj.transform;
        }

        if (sfxSource == null)
        {
            sfxSource = GetComponent<AudioSource>();
            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
            }
        }

        sfxSource.playOnAwake = false;

        moveSpeed = stats.moveSpeed;
    }

    // UPDATED: Now accepts Knockback Force
    public void TakeDamage(float damageAmount, float knockbackForce = 0f, bool appliesSlow = false)
    {
        // weight vai de 0 -> 1
        knockbackForce *= (float)(1.001 - stats.weight);
        currentHealth -= damageAmount;

        if (appliesSlow) moveSpeed = stats.moveSpeed * .5f;

        // 1. Trigger Flash
        if (flashMaterial != null && spriteRenderer != null)
        {
            Flash();
        }

        if (hitClip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(hitClip);
        }

        // 2. Apply Knockback (If force > 0 and we know where the player is)
        if (knockbackForce > 0 && target != null)
        {
            // Calculate direction: FROM Player TO Enemy
            Vector2 direction = (transform.position - target.position).normalized;

            if (knockbackRoutine != null) StopCoroutine(knockbackRoutine);
            knockbackRoutine = StartCoroutine(ApplyKnockback(direction, knockbackForce));
        }

        if (currentHealth <= 0) Die();
    }

    private IEnumerator ApplyKnockback(Vector2 direction, float force)
    {
        bool wasStopped = agent.isStopped;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        float timer = 0f;
        float duration = 0.2f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float currentForce = Mathf.Lerp(force, 0f, timer / duration);
            transform.position += (Vector3)direction * currentForce * Time.deltaTime;
            yield return null;
        }

        if (!wasStopped)
        {
            agent.isStopped = false;
        }
    }

    private void Flash()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = originalMaterial;
        flashRoutine = null;
    }

    protected virtual void Die()
    {
        deathEffect = Instantiate(deathEffect);
        deathEffect.transform.position = GetComponentInChildren<AnimationDestroy>().transform.position;
        
        Destroy(gameObject);
    }

    void Update()
    {
        agent.speed = moveSpeed;
    }
}
