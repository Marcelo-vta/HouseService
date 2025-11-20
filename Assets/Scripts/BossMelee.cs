using UnityEngine;

public class BossMelee : MonoBehaviour
{
    private Boss boss;
    void Start()
    {
        boss = transform.parent.GetComponent<Boss>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boss.inMeleeRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boss.inMeleeRange = false;
        }
    }
}
