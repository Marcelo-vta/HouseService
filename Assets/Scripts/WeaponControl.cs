using Unity.VisualScripting;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public GameObject[] weapons;
    private int currentWeaponId = -1;
    private int newWeaponId = 0;

    public Sprite handFrontSprite;
    public Sprite handBackSprite;

    private Transform activeWeaponTransform;
    private GameObject activeWeapon;

    public PlayerRoll playerRoll;

    private Broom meeleeWeaponScript;
    private PizzaBox rangedWeaponScript;

    private Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activeWeaponTransform = transform.GetChild(0);
        activeWeapon = activeWeaponTransform.gameObject;

        playerTransform = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerRoll.GetRolling())
        {
            if (Input.GetMouseButtonDown(0))
            {
                meeleeWeaponScript = activeWeapon.GetComponent<Broom>();
                rangedWeaponScript = activeWeapon.GetComponent<PizzaBox>();

                if (meeleeWeaponScript != null)
                {
                    meeleeWeaponScript.Fire(playerTransform);
                }

                if (rangedWeaponScript != null)
                {
                    rangedWeaponScript.Fire();
                }
            }

        }
    }
    
}
