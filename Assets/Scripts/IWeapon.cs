using UnityEngine;

public interface IWeapon
{
    void Fire(Transform playerTransform = null);
}