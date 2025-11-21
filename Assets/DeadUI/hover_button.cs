using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverDetection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isHovered = false;
    public Image image;
    public Sprite sprite_hovered;
    public Sprite sprite_default;

    public bool IsHovered
    {
        get { return isHovered; }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = sprite_hovered;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = sprite_default;
    }
}