using UnityEngine;

public class PlayerRoll : MonoBehaviour
{
    private float baseSpeed;
    public float rollMaxSpeed = 40f;       // Max speed reached during roll (C point)
    private float rollDuration;       // How long the roll lasts (frames)

    private float rollTimer = 0f;
    private float currentSpeed;

    public GameObject hurtbox;
    public PlayerMovement playerMovement;
    public Animator animator;
    public AnimationClip anim;

    private Vector2 direction;

    public GameObject hands;

    private PlayerStates playerStates;


    void Start()
    {
        baseSpeed = playerMovement.getSpeed();

        currentSpeed = baseSpeed;
        rollDuration = anim.length;

        playerStates = GetComponent<PlayerStates>();
    }

    void Update()
    {
        if (InputManager.Instance.DashInput)
        {
            Roll();
        }

        if (playerStates.rollingState)
        {
            rollTimer += Time.deltaTime;

            // Normalized time [0, 1]
            float t = rollTimer / rollDuration;
            // Normalized time [0, 1]

            // Mathematical curve (matches your graph’s shape):
            // Starts near max speed and decays faster near the end
            float decay = Mathf.Pow(1 - t, 3) / (t + 0.01f);
            currentSpeed = Mathf.Lerp(baseSpeed, rollMaxSpeed, decay);

            if(t >= 0.8)
            {
                hurtbox.SetActive(true);
            }

            // Stop rolling when done
            if (rollTimer >= rollDuration)
            {
                currentSpeed = baseSpeed;
                playerStates.rollingState = false;
            }
        }

        playerMovement.setSpeed(currentSpeed);
    }

    public void Roll()
    {
        if (playerStates.ableToRoll)
        {
            playerStates.rollingState = true;
            rollTimer = 0f;

            hurtbox.SetActive(false);
            animator.SetTrigger("roll"); 
        }
    }
}
