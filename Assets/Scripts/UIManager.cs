using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject endGamePanel;
    public GameObject pausePanel;
    public GameObject jogoRoot;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isPaused = !pausePanel.activeSelf;

            pausePanel.SetActive(isPaused);
            //jogoRoot.SetActive(!isPaused);

            Time.timeScale = isPaused ? 0f : 1f;

        }
    }

    void FixedUpdate()
    {
        // GameController.gameOver
        if (false)
        {
            endGamePanel.SetActive(true);
            jogoRoot.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        //jogoRoot.SetActive(true);
        Time.timeScale = 1f;
    }
}
