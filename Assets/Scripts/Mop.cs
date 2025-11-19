using UnityEngine;

public class Mop : MonoBehaviour
{
    public Broom broom;
    public Sprite witchBroomSprite;

    void Update()
    {
        if (broom.witch)
        {
            GetComponent<SpriteRenderer>().sprite = witchBroomSprite;
        }
    }
}
