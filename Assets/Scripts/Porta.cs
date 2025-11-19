using UnityEngine;

public class Porta : MonoBehaviour
{
    public enum TipoAnchor { Entrada, Saida }

    [Header("Config")]
    public TipoAnchor tipo = TipoAnchor.Saida;   // defina no Inspector

    private bool jogadorPerto = false;
    private GameObject letraE;                   // referência à imagem "letras_20"
    private TransitionManager tm;

    private PlayerStates playerStates;

    void Start()
    {
        tm = GameObject.FindGameObjectWithTag("TransitionManager")
            .GetComponent<TransitionManager>();
    }

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            if (tipo == TipoAnchor.Saida)
                tm.GoForwardRandom();   // vai pra nova sala aleatória
            else
                tm.GoBack();            // volta pra sala anterior
        }
    }

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
