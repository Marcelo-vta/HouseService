using UnityEngine;

public class Wet : MonoBehaviour
{
    public Broom broom;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        animator.SetBool("witch", broom.witch);
    }
}
