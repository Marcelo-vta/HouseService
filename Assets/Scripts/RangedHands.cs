using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class HandsRotate : MonoBehaviour
{
    private bool handsBack = false;
    private bool invertWeapon = false;
    public SpriteRenderer spriteRenderer;

    public Sprite handBackSprite;
    public Sprite handFrontSprite;

    public GameObject weapon;
    public GameObject handSpriteObject;

    public SpriteRenderer weaponSpriteRenderer;
    public Transform weaponTransform;

    public PlayerRoll playerRoll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponSpriteRenderer = weapon.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {


        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        handSpriteObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        print(rotationZ);

        handsBack = rotationZ > 35 || rotationZ < -145;
        invertWeapon = rotationZ > 35 || rotationZ < -90;

        if (handsBack)
        {
            spriteRenderer.sortingOrder = -1;
            weaponSpriteRenderer.sortingOrder = -2;

            spriteRenderer.sprite = handBackSprite;
        }

        if (!handsBack)
        {
            spriteRenderer.sortingOrder = 10;
            weaponSpriteRenderer.sortingOrder = 9;

            spriteRenderer.sprite = handFrontSprite;
        }

        if (invertWeapon)
        {
            weaponSpriteRenderer.flipY = true;
        }

        if (!invertWeapon)
        {
            weaponSpriteRenderer.flipY = false;
        }

    }
}
