using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonType
    {
        Dash,
        Interact
    }

    [SerializeField] private ButtonType buttonType;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"VirtualButton Down: {buttonType}");
        if (buttonType == ButtonType.Dash)
        {
            GameInput.Instance.SetVirtualDashing(true);
        }
        else if (buttonType == ButtonType.Interact)
        {
            GameInput.Instance.SetVirtualInteracting(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"VirtualButton Up: {buttonType}");
        if (buttonType == ButtonType.Dash)
        {
            GameInput.Instance.SetVirtualDashing(false);
        }
        else if (buttonType == ButtonType.Interact)
        {
            GameInput.Instance.SetVirtualInteracting(false);
        }
    }
}
