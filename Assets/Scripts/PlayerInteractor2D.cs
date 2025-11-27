using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractor2D : MonoBehaviour
{
    private List<IInteractable> interactablesInRange = new List<IInteractable>();
    private IInteractable currentHighlighted;

    private void Update()
    {
        IInteractable closest = GetClosestInteractable();

        if (closest != currentHighlighted)
        {
            if (currentHighlighted != null)
            {
                currentHighlighted.ShowHighlight(false);
            }

            currentHighlighted = closest;

            if (currentHighlighted != null)
            {
                Debug.Log($"PlayerInteractor2D: Highlighted {((MonoBehaviour)currentHighlighted).name}");
                currentHighlighted.ShowHighlight(true);
            }
            else
            {
                Debug.Log("PlayerInteractor2D: No object highlighted");
            }
        }

        if (GameInput.Instance.IsInteracting())
        {
            Debug.Log("PlayerInteractor2D: Interact Input Detected");
            if (currentHighlighted != null)
            {
                currentHighlighted.OnInteract(gameObject);
            }
        }
    }

    private IInteractable GetClosestInteractable()
    {
        IInteractable closest = null;
        float minDistance = float.MaxValue;

        foreach (var interactable in interactablesInRange)
        {
            // Assuming interactable is a MonoBehaviour to get transform
            MonoBehaviour mb = interactable as MonoBehaviour;
            if (mb != null)
            {
                float dist = Vector2.Distance(transform.position, mb.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = interactable;
                }
            }
        }

        return closest;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && !interactablesInRange.Contains(interactable))
        {
            interactablesInRange.Add(interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactablesInRange.Contains(interactable))
        {
            interactable.ShowHighlight(false);
            interactablesInRange.Remove(interactable);
            if (currentHighlighted == interactable)
            {
                currentHighlighted = null;
            }
        }
    }
}
