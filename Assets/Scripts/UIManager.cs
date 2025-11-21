using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject endGamePanel;
    public GameObject pausePanel;
    public GameObject playerStatsUI;
    public GameObject jogoRoot;
    private PlayerStates playerStates;

    void Start()
    {
        playerStates = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStates>();
        Time.timeScale = 1f;
        endGamePanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isPaused = !pausePanel.activeSelf;

            pausePanel.SetActive(isPaused);
            playerStatsUI.SetActive(!isPaused);
            //jogoRoot.SetActive(!isPaused);

            Time.timeScale = isPaused ? 0f : 1f;

        }
    }

    void FixedUpdate()
    {
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        playerStatsUI.SetActive(true);
        //jogoRoot.SetActive(true);
        Time.timeScale = 1f;
    }
}
