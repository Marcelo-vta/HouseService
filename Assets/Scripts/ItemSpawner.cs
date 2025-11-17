using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public string selectedItem;
    public bool isTrap = false;

    private bool interactible = true;

    public GameObject item;

    public GameObject shortcutSuggestion;
    public Stopwatch holdingButton = new Stopwatch();

    private bool interacting = false;
    private bool inRange = true;

    private PlayerStates playerStates;
    private PlayerActions playerActions;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;

            playerStates = other.GetComponent<PlayerStates>();
            playerActions = other.GetComponent<PlayerActions>();

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

    private void Update() {

        if (inRange && interactible)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!interacting)
                {
                    holdingButton.Restart();
                    interacting = true;
                    playerStates.interactingState = true;
                }
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                if (interacting)
                {
                    interacting = false;
                    playerStates.interactingState = false;
                }
            }

            if (interacting && holdingButton.ElapsedTimeSec() > 2)
            {
                Interact(playerStates);
            }
        }

        animator.SetBool("Mimic", isTrap);
    }
    void Interact(PlayerStates states)
    {
        interacting = false;
        interactible = false;

        states.interactingState = false;
        states.interactibleState = false;
        
        animator.SetBool("Open", true);

        if (!isTrap)
        {
            playerActions.ObtainItem(selectedItem);
        }
        else
        {
            playerActions.Scare();
        }
    }
}
