using UnityEngine;

public class DebugSystemCheck : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating(nameof(RunCheck), 1f, 1f);
    }

    private void RunCheck()
    {
        Debug.Log("--- SYSTEM CHECK START ---");

        // 1. Check GameInput
        if (GameInput.Instance == null)
        {
            Debug.LogError("CRITICAL: GameInput.Instance is NULL!");
        }
        else
        {
            Debug.Log($"GameInput: IsMobileMode={GameInput.Instance.IsMobileMode}, IsMobileActive={GameInput.Instance.IsMobileActive()}, IsInteracting={GameInput.Instance.IsInteracting()}");
        }

        // 2. Check Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("CRITICAL: No object with tag 'Player' found!");
        }
        else
        {
            Debug.Log($"Player found: {player.name}");
            
            // 3. Check PlayerInteractor2D
            var interactor = player.GetComponent<PlayerInteractor2D>();
            if (interactor == null)
            {
                Debug.LogWarning($"!!! ATTENTION !!! PlayerInteractor2D is MISSING on {player.name}. You MUST add this component to the Player object.");
            }
            else
            {
                Debug.Log($"PlayerInteractor2D: FOUND! Enabled={interactor.enabled}.");
            }
        }

        Debug.Log("--- SYSTEM CHECK END ---");
    }
}
