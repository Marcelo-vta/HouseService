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
        Vector2 lookInput = InputManager.Instance.LookInput;
        Vector2 difference;

        if (lookInput != Vector2.zero)
        {
            // Joystick / Gamepad Aiming
            difference = lookInput.normalized;
        }
        else
        {
            // Mouse Aiming (Fallback)
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            difference = (Vector2)(worldMousePos - transform.position);
            difference.Normalize();
        }

        animator.SetFloat("mouseX", difference.x);
        animator.SetFloat("mouseY", difference.y);

        if (difference.x == 0)
        {
            direction = 1;
        }
        else
        {
            direction = difference.x / Math.Abs(difference.x);
        }
    }
}
