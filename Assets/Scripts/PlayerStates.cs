using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    public float health;
    public int max_health;
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

    public List<string> powerUps;

    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        handsUsable = !( rollingState || interactingState || obtainingState || hurtState || deathState || scaredState );
        ableToWalk = !( rollingState || interactingState || obtainingState || deathState || scaredState );
        ableToRoll = !( obtainingState || interactingState || deathState || hurtState || rollingState || scaredState ) && walkingState;
        ableToRotate = !( obtainingState || interactingState || deathState || hurtState || rollingState  || scaredState );

        playerAnimator.SetBool("interacting", interactingState);
        playerAnimator.SetBool("obtaining", obtainingState);
        playerAnimator.SetBool("hurt", hurtState);
        playerAnimator.SetBool("scared", scaredState);

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
