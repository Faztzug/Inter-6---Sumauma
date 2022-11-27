using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    public GameObject alma1;
    public GameObject alma2;

    public Image healthBar;
    public GameObject warningAnim;
    public GameObject gameOver;
    public GameObject pauseMenu;
    public GameObject fpsCounter;
    public Book book;
    // public GameObject pauseMainMenu;
    // public GameObject pauseSettingsMenu;
    [SerializeField] private Image planta;
    [SerializeField] private Image animal;

    [SerializeField] private Sprite spriteAnimal1ON;
    [SerializeField] private Sprite spriteAnimal1OFF;

    [SerializeField] private Sprite spritePlanta1ON;
    [SerializeField] private Sprite spritePlanta1OFF;

    [SerializeField] private Sprite spriteAnimal2ON;
    [SerializeField] private Sprite spriteAnimal2OFF;

    [SerializeField] private Sprite spritePlanta2ON;
    [SerializeField] private Sprite spritePlanta2OFF;

    [SerializeField] private Sprite spriteAnimal3ON;
    [SerializeField] private Sprite spriteAnimal3OFF;

    [SerializeField] private Sprite spritePlanta3ON;
    [SerializeField] private Sprite spritePlanta3OFF;

    private void Awake() 
    {
        gameOver.SetActive(false);
        pauseMenu.SetActive(false);
    }

    private void Start()
    {
        GetColectableImages();
        UpdateFPSCounterState();

        GameState.OnSettingsUpdated += UpdateFPSCounterState;
    }

    private void OnDestroy() 
    {
        GameState.OnSettingsUpdated -= UpdateFPSCounterState;
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
        // pauseMainMenu.SetActive(true);
        book.gameObject.SetActive(true);
        book.ResetPagesToStart();
        book.gameObject.SetActive(true);
        Time.timeScale = 0f;
        GameState.isGamePaused = true;
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        // pauseSettingsMenu.SetActive(false);
        book.gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameState.isGamePaused = false;
    }

    public void OpenOptions()
    {
        book.FlipPages(Book.FlipDirection.Right);
    }
    public void CloseOptions()
    {
        // pauseSettingsMenu.SetActive(false);
        // pauseMainMenu.SetActive(true);
    }

    private void UpdateFPSCounterState()
    {
        fpsCounter.SetActive(GameState.SaveData.showFPS);
    }

    public void VoltarMenu()
    {
        GameState.LoadScene("Menu inicial");
    }

    public void GetColectableImages()
    {
        if(GameState.GetSceneName() == "Fase 1")
        {
            if (GameState.animalColetadoNaFase)
            { animal.sprite = spriteAnimal1ON;
                alma1.SetActive(true);
            }
            else animal.sprite = spriteAnimal1OFF;

            if (GameState.plantaColetadaNaFase)
            {
                planta.sprite = spritePlanta1ON;
                alma2.SetActive(true);
            }
            else planta.sprite = spritePlanta1OFF;
        }
        else if(GameState.GetSceneName() == "Fase 2")
        {
            if(GameState.animalColetadoNaFase) animal.sprite = spriteAnimal2ON;
            else animal.sprite = spriteAnimal2OFF;

            if(GameState.plantaColetadaNaFase) planta.sprite = spritePlanta2ON;
            else planta.sprite = spritePlanta2OFF;
        }
        else if(GameState.GetSceneName() == "Fase 3")
        {
            if(GameState.animalColetadoNaFase) animal.sprite = spriteAnimal3ON;
            else animal.sprite = spriteAnimal3OFF;

            if(GameState.plantaColetadaNaFase) planta.sprite = spritePlanta3ON;
            else planta.sprite = spritePlanta3OFF;
        }
    }
}
