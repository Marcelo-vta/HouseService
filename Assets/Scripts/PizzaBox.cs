using System.Linq;
using UnityEngine;

public class PizzaBox : MonoBehaviour
{
    public GameObject pizzaPrefab;
    public GameObject pepperoniPrefab;

    public Transform hand;
    public Transform firePoint;

    public float fireForce = 20f;
    public float innacuracyConstant = 30f;

    [Range(1,0)]
    public float accuracy;

    public GameObject cheese;
    public GameObject spicy;
    public GameObject pepperoni;


    private int throwingPepperoni = 0;
    private Stopwatch pepperoniTimer = new Stopwatch();

    private PlayerStates playerStates;

    public void Start()
    {
        playerStates = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerStates>();

    }

    public void Fire(Transform playerTransform = null)
    {

        GameObject bullet = ThrowProjectile(pizzaPrefab, accuracy);

        if (playerStates.powerUps.Contains("pepperoni"))
        {
            print("pepperoni shot");

            throwingPepperoni = 3;

            pepperoniTimer.Restart();
        }

    }

    void Update()
    {
        if (playerStates.powerUps.Contains("cheese"))
        {
            cheese.SetActive(true);
        }

        if (playerStates.powerUps.Contains("spicy"))
        {
            spicy.SetActive(true);
        }

        if (playerStates.powerUps.Contains("pepperoni"))
        {
            pepperoni.SetActive(true);
        }

        if (throwingPepperoni > 0)
        {
            if (.5f - pepperoniTimer.ElapsedTimeSec() < throwingPepperoni * .1f)
            {
                throwingPepperoni -= 1;

                ThrowProjectile(pepperoniPrefab, accuracy - .1f);
            }
        }
    }

    GameObject ThrowProjectile(GameObject projectile, float shotAccuracy)
    {

        float inaccuracy = 1f - shotAccuracy;
        inaccuracy = Random.Range(-inaccuracy, inaccuracy);

        print(inaccuracy);

        float shotOffset = inaccuracy * innacuracyConstant;

        Quaternion offset = Quaternion.Euler(0, 0, shotOffset);
        Quaternion finalRotation = transform.rotation * offset;

        GameObject bullet = Instantiate(projectile, firePoint.position, finalRotation);

        bullet.GetComponent<Rigidbody2D>().AddForce(
            bullet.transform.right * fireForce,
            ForceMode2D.Impulse
        );

        return bullet;
    }


}
