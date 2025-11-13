using NUnit.Framework.Constraints;
using Unity.Mathematics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject slashPrefab;

    public Transform hand;
    public Transform firePoint;

    public float fireForce = 20f;

    public bool melee = false;


    public void Fire(Transform playerTransform = null)
    {
        if (!melee)
        {
            Quaternion offset = Quaternion.Euler(0, 0, 0);
            Quaternion finalRotation = transform.rotation * offset;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, finalRotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * fireForce, ForceMode2D.Impulse);
        }

        if (melee)
        {
            Instantiate(slashPrefab, playerTransform);
        }
    }
}
