using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using Unity.VisualScripting;


public class Boss : MonoBehaviour
{
    public GameObject meleePivot;
    public GameObject meleeTrigger;

    [HideInInspector]
    public bool inMeleeRange;
    
    public float dashSpeed = 10f;
    public float dashAcc = 0f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 2f;
    public float dashStopDist = 3f;
    public float dashAccConversion;

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


    private float distanceMagnitude;

    private bool dashingState = false;
    private bool crossedRadius = false;

    private Stopwatch meleeTimer = new Stopwatch();
    private Stopwatch rangedTimer = new Stopwatch();
    private Stopwatch spawnerTimer = new Stopwatch();

    private Stopwatch attackTimer = new Stopwatch();

    List<string> attacks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bossRb = GetComponent<Rigidbody2D>();

        dashAccConversion = -(player.transform.position - transform.position).magnitude / (dashStopDist * 2);
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
        bossPosition = transform.position;

        positionDiff = playerPosition-bossPosition;

        distanceMagnitude = positionDiff.magnitude;

        if (attackTimer.ElapsedTimeSec() > 3 && canAttack)
        {
            StartCoroutine(AttackCoroutine());
        }
        ;

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(DashCoroutine());
        }
    }

    void FixedUpdate()
    {
        // bossRb.angularVelocity = 20;
        // bossRb.rotation = 30;
        // batDash(3);
            
    }

    IEnumerator AttackCoroutine()
    {
        canAttack = false;
        isAttacking = true;


        attacks = new List<string>();
        for (int i=0; i < attacks.Count; i++)
        {
            print(attacks[i]);
        }

        if (inMeleeRange && canMelee)
        {
            attacks.Add("melee");
            attacks.Add("melee");
        }
        if (!inMeleeRange && canDash)
        {
            attacks.Add("dash");
        }

        if (canSpawn)
        {
            attacks.Add("spawn");
            attacks.Add("spawn");
        }

        int randomAttackIndex = UnityEngine.Random.Range(0, attacks.Count);
        string attack = attacks [randomAttackIndex];

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

        yield return new WaitForSeconds(.6f);

        meleePivot.SetActive(true);

        yield return new WaitForSeconds(.1f);

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

    IEnumerator DashCoroutine2()
    {
        canDash = false;
        isDashing = true;

        // a_n1 = 0.1 * d_n1
        // v_n1 = v_n0 + a_n
        // d_n1 = d_n0 - v_n1
        Vector2 dash = player.transform.position - transform.position;
        float dist = dash.magnitude;
        dashAcc = 0.1f * dist;

        if (dist <= dashStopDist)
        {   
            dashAcc = 0.1f * dist;
        } else
        {   
            dashAcc = 0.1f * dashAccConversion * dist;
        }

        dashSpeed += dashAcc;


        Vector2 dashDirection = dash.normalized;
        bossRb.linearVelocity = dashDirection * dashSpeed * (float)Math.Sqrt(distanceMagnitude);

        yield return new WaitForSeconds(dashDuration);

        bossRb.linearVelocity = Vector2.zero; // Stop the dash movement
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;

        Vector2 dashDirection = (player.transform.position - transform.position).normalized;
        bossRb.linearVelocity = dashDirection * dashSpeed * (float)Math.Sqrt(distanceMagnitude);

        yield return new WaitForSeconds(dashDuration-(dashDuration/3));

        bossRb.linearVelocity = dashDirection * dashSpeed/2 * (float)Math.Sqrt(distanceMagnitude);

        yield return new WaitForSeconds(dashDuration/3-(dashDuration/5));

        bossRb.linearVelocity = dashDirection * dashSpeed/4 * (float)Math.Sqrt(distanceMagnitude);

        yield return new WaitForSeconds(dashDuration/5);

        bossRb.linearVelocity = Vector2.zero; // Stop the dash movement
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
