// IInteractable.cs
using UnityEngine;

public interface IInteractable
{
    void ShowHighlight(bool show);
    void OnInteract(GameObject interactor);
}
