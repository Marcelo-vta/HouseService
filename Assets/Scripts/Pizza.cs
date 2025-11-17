using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Pizza : MonoBehaviour
{
    private string[] powerUps;
    public float damage;

    public GameObject pepperoni;
    public GameObject cheese;
    public GameObject spicy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    public void SetPowerUps(string[] newPowerUps)
    {
        powerUps = newPowerUps;

        if (powerUps.Contains("pepperoni"))
        {
            pepperoni.SetActive(true);
        }
        
        if (powerUps.Contains("cheese"))
        {
            cheese.SetActive(true);
        }

        if (powerUps.Contains("spicy"))
        {
            spicy.SetActive(true);
        }

    }
}
