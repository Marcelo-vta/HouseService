using Unity.VisualScripting;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public string selectedItem;
    public bool isTrap = false;

    public GameObject item;

    public GameObject shortcutSuggestion;
    public Stopwatch holdingButton = new Stopwatch();

    private bool interacting = false;
    private bool inRange = true;

    private PlayerStates playerStates;
    private PlayerActions playerActions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            playerStates = other.GetComponent<PlayerStates>();
            playerActions = other.GetComponent<PlayerActions>();

            foreach (Transform child in other.transform)
            {
                if (child.CompareTag("UI"))
                {
                    child.GameObject().SetActive(true);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;

            foreach (Transform child in other.transform)
            {
                if (child.CompareTag("UI"))
                {
                    child.GameObject().SetActive(false);
                }
            }
        }
    }

    private void Update() {

        if (inRange)
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

    }
    void Interact(PlayerStates states)
    {
        states.interactingState = false;
        interacting = false;
        
        playerActions.ObtainItem(selectedItem);

    }
}
