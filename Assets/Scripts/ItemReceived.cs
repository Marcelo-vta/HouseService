using System.Collections.Generic;
using UnityEngine;

public class ItemReceived : MonoBehaviour
{
    public string itemName;
    public float rotatingSpeed = 50;

    [Header("PizzaBox Item Sprites")]
    public Sprite spicy;
    public Sprite pepperoni;
    public Sprite cheese;

    [Header("Broom Item Sprites")]
    public Sprite wet;
    public Sprite longHandle;
    public Sprite witch;
    public Sprite mop;

    public Transform lights;
    private Dictionary<string, Sprite> items;
    private SpriteRenderer spriteRenderer;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        items = new Dictionary<string, Sprite>
        {
            {"spicy", spicy},
            {"pepperoni", pepperoni},
            {"cheese", cheese},
            {"wet", wet},
            {"long", longHandle},
            {"witch", witch},
            {"mop", mop}
        };

    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = items[itemName];

        Quaternion offset = Quaternion.Euler(0, 0, -rotatingSpeed*Time.deltaTime);
        Quaternion finalRotation = lights.rotation * offset;

        lights.rotation = finalRotation;
    }
}
