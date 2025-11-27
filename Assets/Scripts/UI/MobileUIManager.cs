using UnityEngine;

public class MobileUIManager : MonoBehaviour
{
    [SerializeField] private GameObject mobileCanvas;

    private void Start()
    {
        UpdateVisibility();
    }

    private void Update()
    {
        // In editor, we might toggle the simulation flag at runtime, so check in Update
#if UNITY_EDITOR
        UpdateVisibility();
#endif
    }

    private void UpdateVisibility()
    {
        if (mobileCanvas != null)
        {
            bool shouldShow = GameInput.Instance.IsMobileMode;
            if (mobileCanvas.activeSelf != shouldShow)
            {
                mobileCanvas.SetActive(shouldShow);
            }
        }
    }
}
