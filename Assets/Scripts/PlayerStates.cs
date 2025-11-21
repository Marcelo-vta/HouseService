using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public int max_health = 4;
    public float health;
    public float insanity;
    private float accuracy;

    public bool cleaner;
    public bool pizzaGuy;

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

    public bool ivulnerability;
    public bool damageable;

    public bool deadState;


    public List<string> powerUps;

    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        health = max_health;
    }

    private void Update()
    {
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


        SetInteractible();
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
}
