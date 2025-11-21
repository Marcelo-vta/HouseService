using UnityEngine;

public class ParentBasedOrder : MonoBehaviour
{

    private SpriteRenderer parentSprite;
    private SpriteRenderer selfSprite;

    public int offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selfSprite = GetComponent<SpriteRenderer>();
        parentSprite = transform.parent.GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        int newSortingOrder = parentSprite.sortingOrder + offset;

        selfSprite.sortingLayerID = parentSprite.sortingLayerID;
        selfSprite.sortingOrder = newSortingOrder;
    }

}
