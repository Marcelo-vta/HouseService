using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public enum JoystickType
    {
        Movement,
        Aim
    }

    [SerializeField] private JoystickType joystickType;
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField] private float handleRange = 100f;

    private Vector2 inputVector;
    private Vector2 initialPosition;

    private void Start()
    {
        initialPosition = background.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
        
        if (joystickType == JoystickType.Aim)
        {
            GameInput.Instance.SetVirtualFiring(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out position))
        {
            position.x = (position.x / background.sizeDelta.x);
            position.y = (position.y / background.sizeDelta.y);

            inputVector = new Vector2(position.x * 2 - 1, position.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            handle.anchoredPosition = new Vector2(inputVector.x * (background.sizeDelta.x / 2), inputVector.y * (background.sizeDelta.y / 2));

            UpdateGameInput();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        
        UpdateGameInput();

        if (joystickType == JoystickType.Aim)
        {
            GameInput.Instance.SetVirtualFiring(false);
        }
    }

    private void UpdateGameInput()
    {
        if (joystickType == JoystickType.Movement)
        {
            GameInput.Instance.SetVirtualMovement(inputVector);
        }
        else if (joystickType == JoystickType.Aim)
        {
            GameInput.Instance.SetVirtualAim(inputVector);
        }
    }
}
