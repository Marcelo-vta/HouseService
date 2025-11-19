using System.Linq;
using UnityEngine;

public class BroomHandle : MonoBehaviour
{
    private PlayerStates playerStates;
    private SpriteRenderer spriteRenderer;

    public Sprite[] defaultHandles;
    public Sprite[] witchHandles;

    private Sprite[] currentHandles;

    void Start()
    {
        playerStates = GetComponentInParent<PlayerStates>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHandles = defaultHandles;
    }
    void Update()
    {
        int extended = playerStates.powerUps.Contains("long") ? 1 : 0;
        currentHandles = playerStates.powerUps.Contains("witch") ? witchHandles : defaultHandles;

        spriteRenderer.sprite = currentHandles[extended];
    }
}
