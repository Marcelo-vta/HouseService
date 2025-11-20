using System.IO;
using UnityEngine;

public class InteractableDistanceFeedback : MonoBehaviour
{   
    public GameObject mensagem;          // Text (TMP) GameObject que mostra "Aperte E"
    public GameObject shiningBorder;      // GameObject da borda brilhante (sprite, image, etc.)

    public float textShowDistance = 2f;   // até essa distância mostra "Aperte E"
    private float glowDistance;            // até essa distância a borda começa a brilhar

    public float pulseSpeed = 2f;         // velocidade do pulso
    public float pulseAmount = 0.1f;     // amplitude do pulso (escala)

    // internals
    private float textShowSqr;
    private float glowSqr;
    private Vector3 borderBaseScale;

    // Player
    private PlayerStates player;

    void Start()
    {
        glowDistance = textShowDistance * 2;
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStates>();

        textShowSqr = textShowDistance * textShowDistance;
        glowSqr = glowDistance * glowDistance;

        if (mensagem != null) mensagem.SetActive(false);
        if (shiningBorder != null)
        {
            shiningBorder.SetActive(false);
            borderBaseScale = shiningBorder.transform.localScale;
        }
    }

    private void Update()
    {
        float sqrDist = (player.gameObject.transform.position - transform.position).sqrMagnitude;

        // Interação com E quando estiver dentro da distância de texto
        if (sqrDist <= textShowSqr && Input.GetKeyDown(KeyCode.E))
        {
            DoInteraction();
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float sqrDist = (player.gameObject.transform.position - transform.position).sqrMagnitude;

        // Mostrar/esconder texto "Aperte E"
        if (mensagem != null)
        {
            bool showButton = sqrDist <= textShowSqr;
            if (mensagem.activeSelf != showButton)
                player.interactibleState = showButton;
        }

        // Glow / Border behavior
        if (shiningBorder != null)
        {
            if (sqrDist <= glowSqr)
            {
                if (!shiningBorder.activeSelf) shiningBorder.SetActive(true);
                // pulso simples por escala
                float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
                shiningBorder.transform.localScale = borderBaseScale * pulse;
                // opcional: também pode alterar material color/alpha aqui
            }
            else
            {
                if (shiningBorder.activeSelf)
                {
                    shiningBorder.SetActive(false);
                    shiningBorder.transform.localScale = borderBaseScale;
                }
            }
        }
    }

    void DoInteraction()
    {
        Debug.Log("Interagiu com " + gameObject.name);
        // coloque aqui o código da interação
    }
}