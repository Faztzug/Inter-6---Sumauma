using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    public Image healthBar;
    public GameObject gameOver;
    public GameObject pauseMenu;
    public GameObject pauseMainMenu;
    public GameObject pauseSettingsMenu;

    private void Awake() 
    {
        gameOver.SetActive(false);
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause") && !GameState.IsPlayerDead)
        {
            if (GameState.isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        pauseMainMenu.SetActive(true);
        Time.timeScale = 0f;
        GameState.isGamePaused = true;
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        pauseSettingsMenu.SetActive(false);
        Time.timeScale = 1f;
        GameState.isGamePaused = false;
    }

    public void OpenOptions()
    {
        pauseSettingsMenu.SetActive(true);
        pauseMainMenu.SetActive(false);
    }
    public void CloseOptions()
    {
        pauseSettingsMenu.SetActive(false);
        pauseMainMenu.SetActive(true);
    }

    public void VoltarMenu()
    {
        GameState.LoadScene("Menu inicial");
    }
}
