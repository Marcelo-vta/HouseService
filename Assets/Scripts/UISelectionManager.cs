using UnityEngine;

public class UISelectionManager : MonoBehaviour
{
    [Header("Referências")]
    public Camera mainCamera;
    public Transform playerTransform;

    [Header("Configurações de distância")]
    public float maxConsiderDistance = 4f;      // ate onde o mouse detecta
    public float clickActivateDistance = 1.5f;  // ate onde pode realmente ativar

    // MenuActions
    public MenuActions menuActions;
    void Start()
    {
        // Se não for atribuída manualmente, pega a camera principal automaticamente
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // clique esquerdo
        {
            HandleMouseClick();
        }
    }

    void HandleMouseClick()
    {
        // Faz um raycast 2D do mouse para o mundo
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider == null) return; // não clicou em nada

        GameObject clicked = hit.collider.gameObject;

        // tenta pegar as interfaces
        IInteractable interactable = clicked.GetComponent<IInteractable>() ?? clicked.GetComponentInParent<IInteractable>();
        IPulsable pulsable = clicked.GetComponent<IPulsable>() ?? clicked.GetComponentInParent<IPulsable>();

        // Calcula distância até o player (se houver)
        float distance = Mathf.Infinity;
        if (playerTransform != null)
            distance = Vector2.Distance(playerTransform.position, clicked.transform.position);

        // intensidade do pulso (0 longe, 1 perto)
        float intensity = 1f - Mathf.Clamp01(distance / maxConsiderDistance);

        // Mostra highlight e pulse
        if (interactable != null)
            interactable.ShowHighlight(true);
        if (pulsable != null)
            pulsable.Pulse(intensity);

        // Se estiver perto o suficiente, executa ação dependendo da tag
        if (distance <= clickActivateDistance)
        {
            switch (clicked.tag)
            {
                case "PlayerUI":
                    HandlePlayerUI(clicked);
                    break;

                case "Upgrade":
                    HandleUpgrade(clicked);
                    break;

                case "Selectable":
                    HandleSelectable(clicked);
                    break;

                default:
                    Debug.Log($"Clicou em {clicked.name} sem ação definida para tag '{clicked.tag}'");
                    break;
            }

            // Também chama OnInteract() se o objeto tiver
            interactable?.OnInteract(playerTransform.gameObject);
        }
        else
        {
            Debug.Log($"Muito longe para interagir com {clicked.name}. Distância: {distance:F2}");
        }
    }

    // ===============================
    // Funções específicas por TAG
    // ===============================
    void HandlePlayerUI(GameObject go)
    {
        Debug.Log($"[UISelectionManager] Clicou no Player: {go.name}");
        menuActions.IniciaJogo();
    }

    void HandleUpgrade(GameObject go)
    {
        Debug.Log($"[UISelectionManager] Clicou em Upgrade: {go.name}");
        // código para aplicar ou mostrar detalhes do upgrade
    }

    void HandleSelectable(GameObject go)
    {
        Debug.Log($"[UISelectionManager] Clicou em Selectable: {go.name}");
        // pode ser usado para interações genéricas de UI
    }
}