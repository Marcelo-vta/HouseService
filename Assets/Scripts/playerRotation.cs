using UnityEngine;
using System;
using NUnit.Framework.Interfaces;

public class playerRotation : MonoBehaviour
{
    private Vector2 mousePos;
    public Animator animator;
    private float direction;

    public PlayerRoll playerRoll;
    private PlayerStates playerStates;

    public PlayerMovement playerMovement;
    public bool isUI = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!isUI) playerStates = transform.parent.GetComponent<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isUI)
        {
            if (playerStates.ableToRotate)
            {
                rotatePlayer();
            }
            
            if (playerStates.rollingState)
            {
                direction = playerMovement.GetMovement().x;

                if (direction != 0)
                {
                    direction = direction / Math.Abs(direction);
                }
                else
                {
                    direction = 1;
                }
            } 
        }
        else
        {
            rotatePlayer();
        }
    }
    void rotatePlayer()
    {
        Vector3 difference;
        if (GameInput.Instance.IsMobileActive())
        {
            difference = GameInput.Instance.GetAim();
        }
        else
        {
            difference = Camera.main.ScreenToWorldPoint(GameInput.Instance.GetPointerPosition()) - transform.position;
        }
        // If using mobile and no input, keep last rotation
        if (GameInput.Instance.IsMobileActive() && difference == Vector3.zero)
        {
            return;
        }

        difference.Normalize();

        animator.SetFloat("mouseX", difference.x);
        animator.SetFloat("mouseY", difference.y);

        if (difference.x == 0)
        {
            // Keep previous direction if x is 0, or default to 1 if we really want
            // But usually we just want to update if x is non-zero
        }
        else
        {
            direction = difference.x / Math.Abs(difference.x);
        }
    }
}
