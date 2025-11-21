using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


public class Boss : MonoBehaviour, IDamageable
{
    public GameObject meleePivot;
    public GameObject meleeTrigger;
    public GameObject dashHitbox;
    public EnemyStats stats;

    public float currentHealth;


    [HideInInspector]
    public bool inMeleeRange;

    public float dashSpeed = 10f;
    public float dashAcc = 0f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 2f;
    public float dashStopDist = 3f;
    public float dashAccConversion;

    [SerializeField] protected Transform target;

    private GameObject player;
    private Vector3 playerPosition;
    private Vector3 bossPosition;

    private Vector3 positionDiff;
    private Rigidbody2D bossRb;

    private bool canAttack = true;
    private bool isAttacking = true;

    private bool canDash = true;
    private bool isDashing = false;

    private bool canSpawn = true;
    private bool isSpawning = true;

    private bool canMelee = true;
    private bool isMeleeing = true;

    private bool canTakeDamage = true;
    private bool isTakingDamage = true;

    private bool canDamageAnimation = true;

    private float distanceMagnitude;

    private bool dashingState = false;
    private bool crossedRadius = false;

    private Stopwatch meleeTimer = new Stopwatch();
    private Stopwatch rangedTimer = new Stopwatch();
    private Stopwatch spawnerTimer = new Stopwatch();

    private Stopwatch attackTimer = new Stopwatch();
    private Animator animator;
    private NavMeshAgent agent;

    List<string> attacks;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip meleeAttackClip;
    [SerializeField] private AudioClip dashClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        player = GameObject.FindGameObjectWithTag("Player");
        bossRb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        currentHealth = stats.maxHealth;

        dashAccConversion = -(player.transform.position - transform.position).magnitude / (dashStopDist * 2);

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        audioSource.playOnAwake = false;

        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) target = playerObj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
        bossPosition = transform.position;

        positionDiff = playerPosition - bossPosition;

        distanceMagnitude = positionDiff.magnitude;

        if (attackTimer.ElapsedTimeSec() > 3 && canAttack)
        {
            StartCoroutine(AttackCoroutine());
        }

        if (!isAttacking)
        {
            HandleChase();
        }


        Vector2 playerDirection = (player.transform.position - transform.position).normalized;

        Vector3 currentScale = animator.gameObject.transform.localScale;
        currentScale.x = playerDirection.x / Math.Abs(playerDirection.x);

        animator.gameObject.transform.localScale = currentScale;
        animator.SetFloat("health", currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(deadCoroutine());
        }

        // NÃO usamos mais HandleAttackAudio aqui
    }

    private void HandleChase()
    {
        if (agent.enabled) 
        {
            animator.SetBool("isMoving", true);
            agent.SetDestination(target.position);
            agent.isStopped = false;
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void FixedUpdate()
    {

    }

    IEnumerator AttackCoroutine()
    {
        canAttack = false;
        isAttacking = true;

        agent.isStopped = true; 
        agent.ResetPath();


        attacks = new List<string>();
        for (int i = 0; i < attacks.Count; i++)
        {
            print(attacks[i]);
        }

        if (inMeleeRange && canMelee && currentHealth > 0)
        {
            attacks.Add("melee");
            attacks.Add("melee");
        }
        if (!inMeleeRange && canDash && currentHealth > 0)
        {
            attacks.Add("dash");
        }

        if (canSpawn && currentHealth > 0)
        {
            attacks.Add("spawn");
            attacks.Add("spawn");
        }

        int randomAttackIndex = UnityEngine.Random.Range(0, attacks.Count);
        string attack = attacks[randomAttackIndex];

        print("attacking: " + attack);
        switch (attack)
        {
            case "melee":
                StartCoroutine(MeleeCoroutine());
                yield return new WaitUntil(() => !isMeleeing);
                break;
            case "dash":
                StartCoroutine(DashCoroutine());
                yield return new WaitUntil(() => !isDashing);
                break;
            case "spawn":
                StartCoroutine(SpawnCoroutine());
                yield return new WaitUntil(() => !isSpawning);
                break;
            default:
                break;
        }

        canAttack = true;
        isAttacking = false;
        attackTimer.Restart();

        yield return "";
    }

    IEnumerator MeleeCoroutine()
    {
        isMeleeing = true;
        canMelee = false;

        Vector3 difference = playerPosition - transform.position;
        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        meleePivot.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(.4f);

        meleePivot.SetActive(true);

        // SOM DO ATAQUE
        if (audioSource != null && meleeAttackClip != null)
        {
            audioSource.PlayOneShot(meleeAttackClip);
        }

        yield return new WaitForSeconds(.4f);

        animator.SetBool("isAttacking", false);
        meleePivot.SetActive(false);

        canMelee = true;
        isMeleeing = false;

        yield return "";
    }

    IEnumerator SpawnCoroutine()
    {
        isSpawning = true;


        isSpawning = false;
        canSpawn = false;
        yield return new WaitForSeconds(13);
        canSpawn = true;
    }

    IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;

        animator.SetBool("isDashing", true);

        yield return new WaitForSeconds(1);

        dashHitbox.SetActive(true);

        // SOM DO DASH
        if (audioSource != null && dashClip != null)
        {
            audioSource.PlayOneShot(dashClip);
        }

        Vector2 dashDirection = (player.transform.position - transform.position).normalized;
        bossRb.linearVelocity = dashDirection * dashSpeed * (float)Math.Sqrt(distanceMagnitude);

        yield return new WaitForSeconds(dashDuration - (dashDuration / 3));

        bossRb.linearVelocity = dashDirection * dashSpeed / 2 * (float)Math.Sqrt(distanceMagnitude);

        yield return new WaitForSeconds(dashDuration / 3 - (dashDuration / 5));

        bossRb.linearVelocity = dashDirection * dashSpeed / 4 * (float)Math.Sqrt(distanceMagnitude);

        yield return new WaitForSeconds(dashDuration / 5);


        dashHitbox.SetActive(false);
        bossRb.linearVelocity = Vector2.zero; // Stop the dash movement
        animator.SetBool("isDashing", false);

        yield return new WaitForSeconds(1);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void TakeDamage(float damageAmount, float knockbackForce = 0f, bool appliesSlow = false)
    {
        if (canTakeDamage)
        {
            currentHealth -= damageAmount;
        }

        if (canDamageAnimation)
        {
            animator.SetTrigger("hurt");
        }

        if (audioSource != null && hitClip != null && canTakeDamage)
        {
            audioSource.PlayOneShot(hitClip);
        }
    }

    IEnumerator deadCoroutine()
    {
        yield return new WaitForSeconds(1.4f);
        animator.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }


}
