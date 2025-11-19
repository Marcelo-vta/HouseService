using UnityEngine;

public class Pepperoni : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "bulletRange")
        {
            Destroy(gameObject);
        }
    }
}
