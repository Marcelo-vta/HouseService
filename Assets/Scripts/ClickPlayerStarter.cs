using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickPlayerStarter : MonoBehaviour
{
    [Tooltip("C�mera usada para converter mouse->mundo. Deixe vazio para usar Camera.main.")]
    public Camera mainCamera;

    [Tooltip("Tag usada para identificar objetos de jogador")]
    public string playerTag = "Player";

    public GameObject cleaner;
    public GameObject pizzaGuy;

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
            // encontrou um player e pega o PlayerIdentity se existir
            PlayerIdentity id = clicked.GetComponent<PlayerIdentity>() ?? clicked.GetComponentInParent<PlayerIdentity>();

            int pid = -1;
            string pname = "";

            if (id != null)
            {
                pid = id.playerId;
                pname = id.playerName;
                if (pname == "pizzaGuy")
                {
                    Vector3 pos = new Vector3(-79.6100006f, -9.32999992f, 0f);
                    Instantiate(pizzaGuy, pos, Quaternion.identity);
                }
                else if (pname == "cleaner")
                {
                    Vector3 pos = new Vector3(-79.6100006f, -9.32999992f, 0f);
                    Instantiate(cleaner, pos, Quaternion.identity);
                }

                SceneManager.LoadScene(1);
            }

        }
        else
        {
            // clique em outro objeto (nao player) -> opcional: feedback
            // Debug.Log("Clicou em " + clicked.name + " que n�o tem tag Player.");
        }
    }
}