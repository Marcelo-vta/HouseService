using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStates : MonoBehaviour
{
    public int max_health = 4;
    public float health;
    public float insanity;
    public float attackSpeed;

    private float accuracy;

    public string characterType;

    public bool rollingState;
    public bool interactingState;
    public bool obtainingState;
    public bool walkingState;
    public bool hurtState;
    public bool deathState;
    public bool interactibleState;
    public bool scaredState;

    public bool ableToWalk;
    public bool handsUsable;
    public bool ableToRoll;
    public bool ableToRotate;
    public bool ableToAttack = true;

    public bool ivulnerability;
    public bool damageable;

    public bool deadState;
    public bool isSpawning = true;


    public List<string> powerUps;

    private Animator playerAnimator;


    public static PlayerStates Instance;
    private int lastScene = -1;

    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        health = max_health;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && !isSpawning)
        {
            Instance = null;
            Destroy(gameObject);
        }
        if (SceneManager.GetActiveScene().buildIndex == 0) isSpawning = false;
        
        if (SceneManager.GetActiveScene().buildIndex == 2 && lastScene == 1)
        {
            health = max_health;
            insanity = 0;
            powerUps = new List<string>();
        }

        deathState = health <= 0;

        handsUsable = !( rollingState || interactingState || obtainingState || hurtState || deathState || scaredState );
        ableToWalk = !( rollingState || interactingState || obtainingState || deathState || scaredState );
        ableToRoll = !( obtainingState || interactingState || deathState || hurtState || rollingState || scaredState ) && walkingState;
        ableToRotate = !( obtainingState || interactingState || deathState || hurtState || rollingState  || scaredState );
        damageable = !( obtainingState || deathState || hurtState || scaredState || ivulnerability);

        playerAnimator.SetBool("interacting", interactingState);
        playerAnimator.SetBool("obtaining", obtainingState);
        playerAnimator.SetBool("scared", scaredState);
        playerAnimator.SetBool("dead", deathState);

        if (deathState){StartCoroutine(deadCoroutine());}

        SetInteractible();
        lastScene = SceneManager.GetActiveScene().buildIndex;
    }

    System.Collections.IEnumerator deadCoroutine()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);

    }

    private void SetInteractible()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("UI"))
            {
                child.gameObject.SetActive(interactibleState);
            }
        }
    }

    public void AttackCooldown(float attackCooldown)
    {
        StartCoroutine(attackCooldownCoroutine(attackCooldown));
    }

    IEnumerator attackCooldownCoroutine(float attackCooldown)
    {
        float atkSpdModified = (100 + attackSpeed)/100f;

        yield return new WaitForSeconds(attackCooldown / atkSpdModified);
        ableToAttack = true;
    }

}
