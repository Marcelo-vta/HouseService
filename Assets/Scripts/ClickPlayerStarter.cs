using UnityEngine;

public class ClickPlayerStarter : MonoBehaviour
{
    [Tooltip("Câmera usada para converter mouse->mundo. Deixe vazio para usar Camera.main.")]
    public Camera mainCamera;

    [Tooltip("Tag usada para identificar objetos de jogador")]
    public string playerTag = "Player";

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrySelectPlayerByClick();
        }
    }

    void TrySelectPlayerByClick()
    {
        if (mainCamera == null) return;

        Vector2 worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider == null) return;

        GameObject clicked = hit.collider.gameObject;

        if (clicked.CompareTag(playerTag))
        {
            // encontrou um player — pega o PlayerIdentity se existir
            PlayerIdentity id = clicked.GetComponent<PlayerIdentity>() ?? clicked.GetComponentInParent<PlayerIdentity>();

            int pid = -1;
            string pname = "";

            if (id != null)
            {
                pid = id.playerId;
                pname = id.playerName;
                Debug.Log($"Player clicado: {clicked.name} id={pid} name={pname}");
            }
            else
            {
                // fallback: usa a ordem/hierarchy ou name se não houver PlayerIdentity
                Debug.LogWarning("Player clicado sem PlayerIdentity. Usando fallback pelo nome.");
                pname = clicked.name;
            }

            // salva a seleção no GameController (para ser lida na próxima cena)
            GameController.SetSelectedPlayer(pid, pname);

            // chama a função IniciaJogo no seu MenuActions (procura componente)
            var menu = FindObjectOfType<MenuActions>();
            if (menu != null)
            {
                // se o seu IniciaJogo original não aceita parâmetros, apenas chama
                menu.IniciaJogo();

                // Se quiser, você pode criar uma sobrecarga em MenuActions que aceite (int id) e chamar aqui:
                // menu.IniciaJogo(pid);
            }
            else
            {
                // fallback: se você não tiver MenuActions, chama diretamente o GameController e o SceneController
                Debug.LogWarning("MenuActions não encontrado. Chamando GameController.Init() e carregando cena diretamente.");
                GameController.Init();
                SceneController.Instance.LoadScene(1);
            }
        }
        else
        {
            // clique em outro objeto (não player) -> opcional: feedback
            // Debug.Log("Clicou em " + clicked.name + " que não tem tag Player.");
        }
    }
}