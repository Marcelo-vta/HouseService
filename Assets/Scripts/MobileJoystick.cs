using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform background = null;
    [SerializeField] private RectTransform handle = null;
    [SerializeField] private float handleRange = 1f;
    [SerializeField] private bool isLeftJoystick = true; // True for Move, False for Aim

    private Canvas canvas;
    private Camera cam;
    private Vector2 inputVector;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
        Vector2 radius = background.sizeDelta / 2;
        inputVector = (eventData.position - position) / (radius * canvas.scaleFactor);
        
        FormatInput();
        HandleInput(inputVector.magnitude, inputVector.normalized, radius, cam);
        handle.anchoredPosition = inputVector * radius * handleRange;
        
        UpdateInputManager();
    }

    private void FormatInput()
    {
        if (inputVector.magnitude > 1)
        {
            inputVector = inputVector.normalized;
        }
    }

    private void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > 0)
        {
            if (magnitude > 1)
                inputVector = normalised;
        }
        else
        {
            inputVector = Vector2.zero;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        UpdateInputManager();
    }

    private void UpdateInputManager()
    {
        if (InputManager.Instance == null) return;

        if (isLeftJoystick)
        {
            InputManager.Instance.VirtualMoveInput = inputVector;
        }
        else
        {
            InputManager.Instance.VirtualLookInput = inputVector;
        }
    }
}
