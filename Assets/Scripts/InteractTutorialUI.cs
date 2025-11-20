using System.Collections;
using UnityEngine;

public class InteractTutorialUI : MonoBehaviour
{
    [Header("Grupo de UI (no Canvas)")]
    public GameObject tutorialGroup;

    [Header("Elementos de UI")]
    public GameObject keyE;
    public GameObject interactText;

    [Header("Mensagens")]
    public GameObject upgradeMessage;
    public GameObject trapMessage;

    [Header("Configuração")]
    public float messageDuration = 2.5f;

    private PlayerStates playerStates;

    private bool inRange = false;
    private bool showingResult = false;

    void Start()
    {
        playerStates = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerStates>();

        if (tutorialGroup != null)
            tutorialGroup.SetActive(false);

        HideAll();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;


        inRange = true;

        if (tutorialGroup != null)
            tutorialGroup.SetActive(true);

        // Prompt inicial
        if (keyE != null) keyE.SetActive(true);
        if (interactText != null) interactText.SetActive(true);

        if (upgradeMessage != null) upgradeMessage.SetActive(false);
        if (trapMessage != null) trapMessage.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;


        inRange = false;

        if (!showingResult)
        {
            HideAll();
            if (tutorialGroup != null)
                tutorialGroup.SetActive(false);
        }
    }

    void Update()
    {
        if (playerStates == null) return;
        if (!inRange) return;
        if (showingResult) return;



        // ---------- BAÚ UPGRADE ----------
        if (playerStates.obtainingState)
        {
            StartCoroutine(ShowResult(upgradeMessage));
            return;
        }

        // ---------- BAÚ TRAP ----------
        if (playerStates.scaredState)
        {
            StartCoroutine(ShowResult(trapMessage));
            return;
        }
    }

    IEnumerator ShowResult(GameObject msg)
    {
        showingResult = true;

        // garante que o grupo está ativo mesmo se tivermos escondido antes
        if (tutorialGroup != null)
            tutorialGroup.SetActive(true);

        // some prompt
        if (keyE != null) keyE.SetActive(false);
        if (interactText != null) interactText.SetActive(false);

        // mostra só a mensagem correta
        if (upgradeMessage != null) upgradeMessage.SetActive(false);
        if (trapMessage != null) trapMessage.SetActive(false);
        if (msg != null) msg.SetActive(true);

        yield return new WaitForSeconds(messageDuration);

        HideAll();
        if (tutorialGroup != null)
            tutorialGroup.SetActive(false);

        showingResult = false;

    }

    void HideAll()
    {
        if (keyE != null) keyE.SetActive(false);
        if (interactText != null) interactText.SetActive(false);
        if (upgradeMessage != null) upgradeMessage.SetActive(false);
        if (trapMessage != null) trapMessage.SetActive(false);
    }
}
