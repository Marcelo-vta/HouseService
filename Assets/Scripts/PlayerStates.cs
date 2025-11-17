using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
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
        handsUsable = !(rollingState || interactingState || obtainingState || hurtState || deathState);
        ableToWalk = !(rollingState || interactingState || obtainingState || deathState);
        ableToRoll = !(obtainingState || interactingState || deathState || hurtState || rollingState);
        ableToRotate = !( obtainingState || interactingState || deathState || hurtState || rollingState );

        playerAnimator.SetBool("interacting", interactingState);
        playerAnimator.SetBool("obtaining", obtainingState);
        playerAnimator.SetBool("hurt", hurtState);
    }
}
