using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public GameObject[] weapons;
    private int currentWeaponId = -1;
    private int newWeaponId = 0;

    public GameObject activeWeapon = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            newWeaponId = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            newWeaponId = 1;
        }

        if (currentWeaponId != newWeaponId)
        {
            Destroy(activeWeapon);
            activeWeapon = Instantiate(weapons[newWeaponId], transform);

            currentWeaponId = newWeaponId;
        }

        print("current: " + currentWeaponId);
        print("new: " + newWeaponId);    
    }
    
    private void SwapWeapons(GameObject[] weapons, int first_index, int second_index)
    {
        GameObject temp = weapons[first_index];

        weapons[first_index] = weapons[second_index];
        weapons[second_index] = temp;
    }
}
