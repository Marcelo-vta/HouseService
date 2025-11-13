using UnityEngine;
using System.Collections;

public class GhostAttack : MonoBehaviour
{
    private bool isAttacking = false;
    private Animator animator;
    private float attackTimer = 0f;
    private float attackDuration = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            Attack();
        }

        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackDuration > 0 && attackTimer >= attackDuration)
            {
                AttackFinished();
            }
        }
    }

    private void Attack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        attackTimer = 0f;
        StartCoroutine(SetAttackDuration());
    }

    private IEnumerator SetAttackDuration()
    {
        yield return null; // Wait for the state to transition
        attackDuration = animator.GetCurrentAnimatorStateInfo(0).length;
    }

    private void AttackFinished()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        attackDuration = 0f;
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }
}
