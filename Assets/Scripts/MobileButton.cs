using UnityEngine;
using UnityEngine.EventSystems;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonType { Dash, Interact, Fire }
    public ButtonType buttonType;

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.Dash:
                InputManager.Instance.TriggerMobileDash();
                break;
            case ButtonType.Interact:
                InputManager.Instance.TriggerMobileInteract();
                break;
            case ButtonType.Fire:
                InputManager.Instance.MobileFire(true);
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (buttonType)
        {
            case ButtonType.Fire:
                InputManager.Instance.MobileFire(false);
                break;
        }
    }
}
