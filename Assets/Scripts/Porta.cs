using UnityEngine;

public class Porta : MonoBehaviour
{
    [Header("Configuração da Porta")]
    public string direcao; // Exemplo: "SaidaOeste", "SaidaLeste", "SaidaSul"
    public string salaDestino; // Nome exato do GameObject da sala destino
    public GameObject promptUI; // UI "Aperte E"

    private bool jogadorPerto = false;
    private TransitionManager transitionManager;

    void Start()
    {
        // Busca automática do TransitionManager na cena
        transitionManager = FindObjectOfType<TransitionManager>();
        if (transitionManager == null)
            Debug.LogWarning("⚠️ TransitionManager não encontrado na cena!");

        if (promptUI != null)
            promptUI.SetActive(false);

        Debug.Log($"🚪 Porta configurada: {gameObject.name} → Sala destino: {salaDestino} ({direcao})");
    }

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"🌀 Indo para {salaDestino} pela {direcao}");
            TransitionManager tm = FindObjectOfType<TransitionManager>();
            if (tm != null)
                tm.StartTransition(salaDestino, direcao);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            Debug.Log($"👣 Player entrou no raio da porta: {gameObject.name}");
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            Debug.Log($"🚶 Player saiu do raio da porta: {gameObject.name}");
            if (promptUI != null)
                promptUI.SetActive(false);
        }
    }
}
