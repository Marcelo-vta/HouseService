using UnityEngine;

public class MovementUI : MonoBehaviour
{
    [Header("Grupo do tutorial (UI dentro do Canvas)")]
    public GameObject tutorialGroup;

    [Header("Ícones das teclas")]
    public GameObject keyW;
    public GameObject keyA;
    public GameObject keyS;
    public GameObject keyD;
    public GameObject keySpace;

    [Header("Textos")]
    public GameObject movementTitle;
    public GameObject dashTitle;

    private bool inRange = false;

    private PlayerStates playerStates;
    private bool wasRollingLastFrame = false;

    void Start()
    {
        playerStates = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStates>();

        // Canvas começa invisível
        tutorialGroup.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        inRange = true;

        // Ativa todo o grupo quando entra no trigger
        tutorialGroup.SetActive(true);

        // Mostra todos os ícones que ainda estiverem ativos
        RefreshUI();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        inRange = false;
        tutorialGroup.SetActive(false);
    }

    void Update()
    {
        UpdateRollingEdge();

        if (!inRange) return;

        // ----------------------
        //   MOVEMENT (WASD)
        // ----------------------

        if (Input.GetKeyDown(KeyCode.W)) keyW.SetActive(false);
        if (Input.GetKeyDown(KeyCode.A)) keyA.SetActive(false);
        if (Input.GetKeyDown(KeyCode.S)) keyS.SetActive(false);
        if (Input.GetKeyDown(KeyCode.D)) keyD.SetActive(false);

        // Se TODOS os ícones do movement sumiram → apaga o texto "MOVEMENT"
        if (!keyW.activeSelf && !keyA.activeSelf && !keyS.activeSelf && !keyD.activeSelf)
        {
            movementTitle.SetActive(false);
        }

        // ----------------------
        //   DASH (SPACE + roll)
        // ----------------------

        // Quando SPACE for pressionado → ícone some
        if (Input.GetKeyDown(KeyCode.Space))
        {
            keySpace.SetActive(false);
        }

        // Quando o player DER dash real → escondemos o texto DASH
        if (StartedRollingThisFrame())
        {
            keySpace.SetActive(false);
            dashTitle.SetActive(false);
        }

        // Se o SPACE já estiver apagado → apaga DASH também
        if (!keySpace.activeSelf)
        {
            dashTitle.SetActive(false);
        }
    }

    // Atualiza UI quando entrar de novo na área
    void RefreshUI()
    {
        // MovementTitle só aparece se ainda tem teclas para mostrar
        movementTitle.SetActive(
            keyW.activeSelf || keyA.activeSelf || keyS.activeSelf || keyD.activeSelf
        );

        // DashTitle só aparece se SPACE ainda está ativo
        dashTitle.SetActive(keySpace.activeSelf);
    }

    // ----------------------
    // Rolling detection
    // ----------------------

    void UpdateRollingEdge()
    {
        bool isRollingNow = playerStates != null && playerStates.rollingState;
        wasRollingLastFrame = isRollingNow;
    }

    bool StartedRollingThisFrame()
    {
        if (playerStates == null) return false;

        bool isRolling = playerStates.rollingState;
        bool startedNow = isRolling && !wasRollingLastFrame;
        wasRollingLastFrame = isRolling;
        return startedNow;
    }
}
