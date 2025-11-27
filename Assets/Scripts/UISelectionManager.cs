using UnityEngine;

public class UISelectionManager : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;

    private void Update()
    {
        if (GameInput.Instance.IsPointerDownThisFrame())
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        Vector2 pointerPos = GameInput.Instance.GetPointerPosition();
        if (mainCamera == null) mainCamera = Camera.main;

        Vector2 worldPoint = mainCamera.ScreenToWorldPoint(pointerPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null)
        {
            GameObject clicked = hit.collider.gameObject;
            Debug.Log($"Clicked on: {clicked.name} with tag {clicked.tag}");
            
            // Add your selection logic here
            // Example:
            // if (clicked.CompareTag("Player")) { ... }
        }
    }
}
