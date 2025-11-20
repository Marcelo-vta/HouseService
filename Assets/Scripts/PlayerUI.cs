using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class MouseProximityInteractor2D : MonoBehaviour
{
    [Header("Refs")]
    public GameObject shiningBorder;    // objeto que representa a borda/brilho (pode ser um Sprite)
    [Header("Proximity (pixels)")]
    public float maxScreenDistance = 120f; // distância em pixels para começar o efeito
    public float minScaleMultiplier = 1f;  // escala quando longe
    public float maxScaleMultiplier = 1.25f; // escala quando perto

    [Header("Smooth")]
    public float smoothSpeed = 12f;     // quão rápido interpola (maior = mais rápido)

    [Header("Tag-based click events (opcionais)")]
    public UnityEvent startGameEvent;    // chamado se tag == "PlayerUI"
    public UnityEvent onPickUp;          // chamado se tag == "Selectable" (além de SendMessage)
    public UnityEvent defaultClickEvent; // caso a tag não seja nenhuma das acima

    // internals
    private Vector3 borderBaseScale;
    private Vector3 currentScale;
    private Camera mainCam;
    private Collider2D col2d;

    void Start()
    {
        mainCam = Camera.main;
        col2d = GetComponent<Collider2D>();

        if (shiningBorder != null)
        {
            borderBaseScale = shiningBorder.transform.localScale;
            currentScale = borderBaseScale * minScaleMultiplier;
            shiningBorder.transform.localScale = currentScale;
            shiningBorder.SetActive(false);
        }
    }

    void Update()
    {
        UpdateMouseProximityEffect();
        HandleMouseClick();
    }

    // Usa Vector2 / 2D screen positions
    private void UpdateMouseProximityEffect()
    {
        if (shiningBorder == null || mainCam == null) return;

        // posição do objeto em screen space (pixels)
        Vector2 screenPos = mainCam.WorldToScreenPoint(transform.position);

        // se estiver atrás da camera (z < 0) escondemos
        Vector3 worldToCam = mainCam.WorldToScreenPoint(transform.position);
        if (worldToCam.z < 0f)
        {
            if (shiningBorder.activeSelf) shiningBorder.SetActive(false);
            return;
        }

        Vector2 mousePos = Input.mousePosition;

        // distância em pixels entre mouse e centro do objeto
        float distPixels = Vector2.Distance(mousePos, screenPos);

        // normaliza 0..1 (0 = centro, 1 = na maxScreenDistance ou mais longe)
        float t = Mathf.Clamp01(distPixels / Mathf.Max(0.0001f, maxScreenDistance));
        float proximity = 1f - t; // 0 (longe) .. 1 (perto)

        bool shouldBeActive = proximity > 0f;

        if (shouldBeActive)
        {
            if (!shiningBorder.activeSelf) shiningBorder.SetActive(true);

            float scaleMul = Mathf.Lerp(minScaleMultiplier, maxScaleMultiplier, proximity);
            Vector3 targetScale = borderBaseScale * scaleMul;

            currentScale = Vector3.Lerp(currentScale, targetScale, Time.deltaTime * smoothSpeed);
            shiningBorder.transform.localScale = currentScale;
        }
        else
        {
            if (shiningBorder.activeSelf)
            {
                // reset suave à escala base
                currentScale = Vector3.Lerp(currentScale, borderBaseScale * minScaleMultiplier, Time.deltaTime * smoothSpeed);
                shiningBorder.transform.localScale = currentScale;

                // se já estiver praticamente resetada, desliga para poupar draw
                if (Vector3.SqrMagnitude(shiningBorder.transform.localScale - borderBaseScale * minScaleMultiplier) < 0.0001f)
                {
                    shiningBorder.transform.localScale = borderBaseScale * minScaleMultiplier;
                    shiningBorder.SetActive(false);
                }
            }
        }
    }

    // Detecta clique do mouse sobre o collider2D
    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) // botão esquerdo
        {
            if (mainCam == null || col2d == null) return;

            Vector2 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);

            // OverlapPoint encontra o collider2D que contém o ponto
            Collider2D hit = Physics2D.OverlapPoint(mouseWorld);
            if (hit != null && hit == col2d)
            {
                // O click foi neste objeto -> faz a ação conforme a tag
                DoClickActionByTag();
            }
        }
    }

    private void DoClickActionByTag()
    {
        string myTag = gameObject.tag;

        if (myTag == "PlayerUI")
        {
            // Inicia o jogo -> expõe UnityEvent para ligar ao teu método StartGame
            if (startGameEvent != null)
                startGameEvent.Invoke();
            else
                Debug.Log("StartGameEvent não atribuído no Inspetor para " + gameObject.name);
        }
        else if (myTag == "Selectable")
        {
            // Tenta invocar uma função PickUp no próprio GameObject (assume que já tens essa função)
            // SendMessage é permissivo: se não existir, apenas avisa (ou usa opcionalmente RequireReceiver)
            try
            {
                // chama método PickUp() se existir
                gameObject.SendMessage("PickUp", SendMessageOptions.DontRequireReceiver);
            }
            catch { /* não deverá acontecer com DontRequireReceiver */ }

            // também dispara o UnityEvent para ligações via Inspector
            if (onPickUp != null)
                onPickUp.Invoke();
        }
        else
        {
            // acção padrão
            if (defaultClickEvent != null)
                defaultClickEvent.Invoke();
            else
                Debug.Log("Clicked on " + gameObject.name + " with tag '" + myTag + "', mas nenhum evento padrão está definido.");
        }
    }

    // Método público para forçar invocação por código (útil em testes)
    public void InvokeClickAction()
    {
        DoClickActionByTag();
    }
}