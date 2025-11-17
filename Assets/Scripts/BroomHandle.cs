using System.Linq;
using UnityEngine;

public class BroomHandle : MonoBehaviour
{
    public Broom broom;
    private SpriteRenderer spriteRenderer;

    public Sprite[] defaultHandles;
    public Sprite[] witchHandles;

    private Sprite[] currentHandles;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHandles = defaultHandles;
    }
    void Update()
    {
        int extended = broom.powerUps.Contains("long") ? 1 : 0;
        currentHandles = broom.powerUps.Contains("witch") ? witchHandles : defaultHandles;

        spriteRenderer.sprite = currentHandles[extended];
    }
}
