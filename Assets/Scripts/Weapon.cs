using Unity.Mathematics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform hand;
    public Transform firePoint;
    public float fireForce = 20f;


    public void Fire()
    {
        Quaternion offset = Quaternion.Euler(0, 0, -90f);
        Quaternion finalRotation = transform.rotation * offset;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, finalRotation );
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * fireForce, ForceMode2D.Impulse);
    }
}
