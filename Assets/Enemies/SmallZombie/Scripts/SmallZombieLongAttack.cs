using UnityEngine;

public class SmallZombieLongAttack : MonoBehaviour
{
    private float baseSpeed;
    public float longAttackMaxSpeed = 40f;       // Max speed reached during long attack (C point)
    private float longAttackDuration;       // How long the long attack lasts (frames)
    private bool isLongAttacking = false;

    private float longAttackTimer = 0f;
    private float currentSpeed;

    public SmallZombieMovement smallZombieMovement;
    public Animator animator;
    public AnimationClip anim;

    private Vector2 direction;

    void Start()
    {
        baseSpeed = smallZombieMovement.getSpeed();

        currentSpeed = baseSpeed;
        longAttackDuration = anim.length;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LongAttack();
        }

        if (isLongAttacking)
        {
            longAttackTimer += Time.deltaTime;

            // Normalized time [0, 1]
            float t = longAttackTimer / longAttackDuration;

            // Normalized time [0, 1]

            // Mathematical curve (matches your graph’s shape):
            // Starts near max speed and decays faster near the end
            float decay = Mathf.Pow(1 - t, 3) / (t + 0.01f);
            currentSpeed = Mathf.Lerp(baseSpeed, longAttackMaxSpeed, decay);

            // Stop rolling when done
            if (longAttackTimer >= longAttackDuration)
            {
                isLongAttacking = false;
                currentSpeed = baseSpeed;
            }
        }

        smallZombieMovement.setSpeed(currentSpeed);

        animator.SetBool("isLongAttacking", isLongAttacking);
    }

    public void LongAttack()
    {
        if (!isLongAttacking)
        {
            isLongAttacking = true;
            longAttackTimer = 0f;
        }
    }

    public bool GetLongAttacking()
    {
        return isLongAttacking;
    }
}
