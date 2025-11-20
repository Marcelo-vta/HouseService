using UnityEngine;

public class AttackTutorialUI : MonoBehaviour
{
    [Header("Grupo de UI (Canvas → AttackTutorialUI)")]
    public GameObject tutorialGroup;   // UI completa (pai)

    [Header("Elementos de UI")]
    public GameObject attackText;      // Texto: "ATTACK"
    public GameObject mouseIcon;       // Sprite do mouse
    public GameObject enemyPrefab;     // Prefab do inimigo (opcional)

    private GameObject spawnedEnemy;
    private bool inRange = false;

    void Start()
    {
        // Começa tudo desligado
        if (tutorialGroup != null)
            tutorialGroup.SetActive(false);

        if (attackText != null) attackText.SetActive(false);
        if (mouseIcon != null) mouseIcon.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        inRange = true;

        // Ativa o grupo de UI
        if (tutorialGroup != null)
            tutorialGroup.SetActive(true);

        if (attackText != null) attackText.SetActive(true);
        if (mouseIcon != null) mouseIcon.SetActive(true);

        // Spawna o inimigo APENAS se tiver prefab configurado
        if (enemyPrefab != null && spawnedEnemy == null)
        {
            spawnedEnemy = Instantiate(enemyPrefab, transform.position + new Vector3(1.5f, 0, 0), Quaternion.identity);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        inRange = false;

        // Some a UI
        if (attackText != null) attackText.SetActive(false);
        if (mouseIcon != null) mouseIcon.SetActive(false);
        if (tutorialGroup != null) tutorialGroup.SetActive(false);

        // Remove inimigo se foi criado pelo tutorial
        if (spawnedEnemy != null)
            Destroy(spawnedEnemy);
    }
}
