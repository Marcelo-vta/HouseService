using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawner : MonoBehaviour, IInteractable
{
    public string selectedItem;
    public bool isTrap = false;

    private bool interactible = true;

    public GameObject item;

    public GameObject shortcutSuggestion;
    public Stopwatch holdingButton = new Stopwatch();

    private bool interacting = false;
    private bool inRange = false;

    private PlayerStates playerStates;
    private PlayerActions playerActions;
    private Animator animator;

    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        loadPlayer();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            playerStates.interactibleState = interactible;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;

            playerStates.interactibleState = false;
        }
    }

    void loadPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        playerStates = player
            .GetComponent<PlayerStates>();
        playerActions = player
            .GetComponent<PlayerActions>();
    }

    // IInteractable Implementation
    public void ShowHighlight(bool show)
    {
        if (shortcutSuggestion != null)
        {
            shortcutSuggestion.SetActive(show);
        }
        
        if (!show)
        {
            // Reset interaction if we walk away
            interacting = false;
            holdingButton.Restart();
            if (playerStates != null) playerStates.interactingState = false;
        }
    }

    public void OnInteract(GameObject interactor)
    {
        // Called every frame while button is held
        if (!interacting)
        {
            interacting = true;
            holdingButton.Restart();
            if (playerStates != null) playerStates.interactingState = true;
        }

        // Check hold time
        if (holdingButton.ElapsedTimeSec() > 2)
        {
            Interact();
        }
    }

    private void LateUpdate()
    {
        // If we stopped holding the button (GameInput check), reset
        // We can check GameInput directly here since we are coupling to it anyway
        if (interacting && !GameInput.Instance.IsInteracting())
        {
            interacting = false;
            holdingButton.Restart();
            if (playerStates != null) playerStates.interactingState = false;
        }
        
        animator.SetBool("Mimic", isTrap);
    }

    public void Interact()
    {
        interacting = false;
        interactible = false;

        if (playerStates != null)
        {
            playerStates.interactingState = false;
            playerStates.interactibleState = false;
        }
        
        animator.SetBool("Open", true);

        if (!isTrap)
        {
            if (playerActions != null) playerActions.ObtainItem(selectedItem);
        }
        else
        {
            if (playerActions != null) playerActions.Scare();
        }
    }
}
