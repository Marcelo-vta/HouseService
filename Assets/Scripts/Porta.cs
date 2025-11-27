using UnityEngine;
using UnityEngine.SceneManagement;

public class Porta : MonoBehaviour, IInteractable
{
    public enum TipoAnchor { Entrada, Saida }

    [Header("Config")]
    public TipoAnchor tipo = TipoAnchor.Saida;   // defina no Inspector

    private bool jogadorPerto = false;
    private GameObject letraE;                   // referência à imagem "letras_20"
    private TransitionManager tm;

    private PlayerStates playerStates;
    public bool tutorial = false;

    void Start()
    {
        tm = GameObject.FindGameObjectWithTag("TransitionManager")
            .GetComponent<TransitionManager>();
    }

    // IInteractable Implementation
    public void ShowHighlight(bool show)
    {
        if (letraE != null)
        {
            letraE.SetActive(show);
        }
    }

    public void OnInteract(GameObject interactor)
    {
        // Execute interaction immediately
        if (tutorial)
        {
            SceneManager.LoadScene(2);    
        }

        if (tipo == TipoAnchor.Saida)
            tm.GoForwardRandom();   // vai pra nova sala aleatória
        else
            tm.GoBack();            // volta pra sala anterior
    }

    // Removed Update() as input is now handled by OnInteract

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerStates = other.gameObject.GetComponent<PlayerStates>();

            jogadorPerto = true;
            playerStates.interactibleState = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            playerStates.interactibleState = false;
        }
    }
}
