using UnityEngine;

public class FollowParentFlip : MonoBehaviour
{
    public bool x;
    public bool y;

    private SpriteRenderer parentSpriteRenderer;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if (x)
        {
            spriteRenderer.flipX = parentSpriteRenderer.flipX;
        }   

        if (y)
        {
            spriteRenderer.flipY = parentSpriteRenderer.flipY;
        }    
    }
}
