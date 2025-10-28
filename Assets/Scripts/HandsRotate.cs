using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HandsRotate : MonoBehaviour
{
    private bool handsBack = false;
    public SpriteRenderer spriteRenderer;

    public Sprite handBackSprite;
    public Sprite handFrontSprite;

    public PlayerRoll playerRoll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        print(rotationZ);

        handsBack = rotationZ > 35 || rotationZ < -145;

        if (handsBack)
        {
            spriteRenderer.sortingOrder = -1;
            spriteRenderer.sprite = handBackSprite;
        }

        if (!handsBack)
        {
            spriteRenderer.sortingOrder = 10;
            spriteRenderer.sprite = handFrontSprite;
        }

    }
}
