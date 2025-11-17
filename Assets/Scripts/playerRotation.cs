using UnityEngine;
using System;

public class playerRotation : MonoBehaviour
{
    private Vector2 mousePos;
    public Animator animator;
    private float direction;

    public PlayerRoll playerRoll;
    private PlayerStates playerStates;

    public PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStates = transform.parent.GetComponent<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStates.ableToRotate)
        {
            mousePos = Input.mousePosition;

            mousePos.x -= Screen.width / 2;
            mousePos.y -= Screen.height / 2;

            mousePos.x /= Screen.width / 2;
            mousePos.y /= Screen.height / 2;

            animator.SetFloat("mouseX", mousePos.x);
            animator.SetFloat("mouseY", mousePos.y);

            if (mousePos.x == 0)
            {
                direction = 1;
            }
            else
            {
                direction = mousePos.x / Math.Abs(mousePos.x);
            }
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
}
