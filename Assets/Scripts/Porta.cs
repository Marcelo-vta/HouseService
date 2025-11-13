using UnityEngine;

public class Porta : MonoBehaviour
{
    public enum TipoAnchor { Entrada, Saida }

    [Header("Config")]
    public TipoAnchor tipo = TipoAnchor.Saida;   // defina no Inspector

    private bool jogadorPerto = false;
    private GameObject letraE;                   // referência à imagem "letras_20"
    private TransitionManager tm;

    void Start()
    {
        tm = FindObjectOfType<TransitionManager>();

        // tenta achar o filho chamado "letras_20"
        Transform letraT = transform.Find("letras_20");
        if (letraT != null)
        {
            letraE = letraT.gameObject;
            letraE.SetActive(false); // começa invisível
        }
    }

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E) && tm != null)
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
            jogadorPerto = true;
            if (letraE != null) letraE.SetActive(true); // mostra a letra E
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            if (letraE != null) letraE.SetActive(false); // esconde a letra E
        }
    }
}
