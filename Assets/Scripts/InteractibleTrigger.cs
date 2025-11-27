using UnityEngine;
using UnityEngine.Events;

public class InteractibleTrigger : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    public bool holdToInteract = true;
    public float holdDuration = 1.0f;
    
    [Header("Visuals")]
    public GameObject highlightObject;
    public GameObject progressBar; // Optional: visual feedback for holding

    [Header("Events")]
    public UnityEvent onInteract;

    private float currentHoldTime = 0f;
    private bool isInteracting = false;
    private float lastInteractTime;

    public void ShowHighlight(bool show)
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(show);
        }
        
        if (!show)
        {
            ResetInteraction();
        }
    }

    public void OnInteract(GameObject interactor)
    {
        lastInteractTime = Time.time;

        if (holdToInteract)
        {
            currentHoldTime += Time.deltaTime;
            
            // Debug feedback
            if (Time.frameCount % 10 == 0) Debug.Log($"InteractibleTrigger: Holding... {currentHoldTime:F2}/{holdDuration}");

            if (currentHoldTime >= holdDuration)
            {
                if (!isInteracting)
                {
                    TriggerInteraction();
                    isInteracting = true; 
                }
            }
        }
        else
        {
            // Instant interaction
            // We use a small cooldown or flag to prevent multiple triggers per frame
            if (!isInteracting)
            {
                TriggerInteraction();
                isInteracting = true;
            }
        }
    }

    private void TriggerInteraction()
    {
        Debug.Log($"Interacted with {gameObject.name}");
        onInteract?.Invoke();
    }

    private void LateUpdate()
    {
        // If OnInteract wasn't called this frame (plus a small buffer), reset
        if (Time.time - lastInteractTime > Time.deltaTime * 2f)
        {
            ResetInteraction();
        }
    }

    private void ResetInteraction()
    {
        currentHoldTime = 0f;
        isInteracting = false;
        // Optional: Reset progress bar
    }
}
