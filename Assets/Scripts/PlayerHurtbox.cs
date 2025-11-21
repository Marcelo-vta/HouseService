using System.Collections;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour, IDamageable
{

    public float ivulnerableTime = .7f;
    public float recoverTime = .5f;

    public PlayerRoll playerRoll;

    public GameObject hands;
    private PlayerStates playerStates;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStates = transform.parent.gameObject.GetComponent<PlayerStates>();
        animator = transform.parent.gameObject.GetComponentInChildren<Animator>();
    }

    public void TakeDamage(float damageAmount, float knockbackForce = 0f, bool appliesSlow = false)
    {
        if (playerStates.damageable)
        {
            playerStates.health -= damageAmount;
            playerStates.hurtState = true;
            animator.SetTrigger("hurt");

            StartCoroutine(HurtCoroutine());
        }
    }

    IEnumerator HurtCoroutine()
    {  
        print("hurtCoroutine");
        playerStates.ivulnerability = true;
        yield return new WaitForSeconds(.8f);

        playerStates.hurtState = false;
        
        yield return new WaitForSeconds(1);
        playerStates.ivulnerability = false;
    }

}
