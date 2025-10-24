using UnityEngine;

public class PlayerRoll : MonoBehaviour
{
    private float baseSpeed;
    public float rollMaxSpeed = 40f;       // Max speed reached during roll (C point)
    private float rollDuration;       // How long the roll lasts (frames)
    private bool isRolling = false;

    private float rollTimer = 0f;
    private float currentSpeed;

    public PlayerMovement playerMovement;
    public Animator animator;
    public AnimationClip anim;

    private Vector2 direction;

    void Start()
    {
        baseSpeed = playerMovement.getSpeed();

        currentSpeed = baseSpeed;
        rollDuration = anim.length;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Roll();
        }

        if (isRolling)
        {
            rollTimer += Time.deltaTime;

            // Normalized time [0, 1]
            float t = rollTimer / rollDuration;

            // Normalized time [0, 1]

            // Mathematical curve (matches your graph’s shape):
            // Starts near max speed and decays faster near the end
            float decay = Mathf.Pow(1 - t, 3) / (t + 0.01f);
            currentSpeed = Mathf.Lerp(baseSpeed, rollMaxSpeed, decay);

            // Stop rolling when done
            if (rollTimer >= rollDuration)
            {
                isRolling = false;
                currentSpeed = baseSpeed;
            }
        }

        playerMovement.setSpeed(currentSpeed);

        animator.SetBool("isRolling", isRolling);
    }

    public void Roll()
    {
        if (!isRolling)
        {
            isRolling = true;
            rollTimer = 0f;
        }
    }

    public bool GetRolling()
    {
        return isRolling;
    }
}
