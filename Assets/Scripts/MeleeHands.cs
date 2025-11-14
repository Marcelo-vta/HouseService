using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class MeleeHands : MonoBehaviour
{
    private bool handsUp = false;
    private bool handsDown = false;
    private bool handsLeft = false;
    private bool handsRight = false;

    private bool invertWeapon = false;

    public SpriteRenderer spriteRenderer;

    private Sprite handBackSprite;
    private Sprite handFrontSprite;

    private WeaponControl parentWeaponControl;

    public GameObject weapon;
    public GameObject handSpriteObject;

    public SpriteRenderer weaponSpriteRenderer;
    public Transform weaponTransform;

    public PlayerRoll playerRoll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentWeaponControl = GetComponentInParent<WeaponControl>();

        handBackSprite = parentWeaponControl.handBackSprite;
        handFrontSprite = parentWeaponControl.handFrontSprite;

        weaponSpriteRenderer = weapon.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        // transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        // handSpriteObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        handsUp = rotationZ > 34 && rotationZ < 140;
        handsDown = rotationZ > -150 && rotationZ < -30;
        handsRight = rotationZ > -30 && rotationZ < 35;
        handsLeft = rotationZ > 140 || rotationZ < -150;

        if (handsUp)
        {
            spriteRenderer.sortingOrder = -1;
            weaponSpriteRenderer.sortingOrder = -2;

            spriteRenderer.sprite = handBackSprite;

            handSpriteObject.transform.localPosition = new Vector3(0.15625f + pixelPositions(4), pixelPositions(1), 0);
        }
        if (handsDown)
        {
            spriteRenderer.sortingOrder = 10;
            weaponSpriteRenderer.sortingOrder = 9;

            spriteRenderer.sprite = handFrontSprite;
            handSpriteObject.transform.localPosition = new Vector3(0.15625f + pixelPositions(-4), 0, 0);
        }

        if (handsLeft)
        {
            spriteRenderer.sortingOrder = -1;
            weaponSpriteRenderer.sortingOrder = -2;

            spriteRenderer.sprite = handBackSprite;
            handSpriteObject.transform.localPosition = new Vector3(0.15625f + pixelPositions(-5), pixelPositions(1), 0);
        }
        if (handsRight)
        {
            spriteRenderer.sortingOrder = 10;
            weaponSpriteRenderer.sortingOrder = 9;

            spriteRenderer.sprite = handFrontSprite;
            handSpriteObject.transform.localPosition = new Vector3(0.15625f + pixelPositions(0), 0, 0);
        }
    }

    private float pixelPositions(int nPixels)
    {
        return 0.0625f * nPixels;
    }
}
