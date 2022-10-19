using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public GameObject pauseMenu;
    public static bool isPaused;

    void Start()
    {
        pauseMenu.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pausou");
            if (isPaused)
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
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Options()
    {

    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene("Menu inicial");
    }
}
