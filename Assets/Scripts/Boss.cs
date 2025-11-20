using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour
{
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
    private Stopwatch attackTimer = new Stopwatch();
    private Rigidbody2D bossRb; 

    private bool canDash = false;
    private bool isDashing = false;

    private float distanceMagnitude;

    private bool dashingState = false;
    private bool crossedRadius = false;

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

        distanceMagnitude = Vector3.Distance(playerPosition, bossPosition);

        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(DashCoroutine2());
        }
    }

    void FixedUpdate()
    {
        // bossRb.angularVelocity = 20;
        // bossRb.rotation = 30;
        // batDash(3);
                
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

        // a_n1 = 0.1 * d_n1
        // v_n1 = v_n0 + a_n
        // d_n1 = d_n0 - v_n1
        Vector2 dashDirection = (player.transform.position - transform.position).normalized;
        bossRb.linearVelocity = dashDirection * dashSpeed * (float)Math.Sqrt(distanceMagnitude);

        yield return new WaitForSeconds(dashDuration);

        bossRb.linearVelocity = Vector2.zero; // Stop the dash movement
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
