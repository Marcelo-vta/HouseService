using UnityEngine;
using System;

public class playerRotation : MonoBehaviour
{
    private Vector2 mousePos;
    public Animator animator;
    private float direction;

    public PlayerRoll playerRoll;
    public PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerRoll.GetRolling())
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

            Vector3 currentScale = transform.localScale;

            currentScale.x = Math.Abs(currentScale.x) * direction;
            transform.localScale = currentScale;
        }
        
        if (playerRoll.GetRolling())
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

            Vector3 currentScale = transform.localScale;

            currentScale.x = Math.Abs(currentScale.x) * direction;
            transform.localScale = currentScale;
        }
    }
}
