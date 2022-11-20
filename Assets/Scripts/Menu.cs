using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button fase1Button;
    [SerializeField] private Button fase2Button;
    [SerializeField] private Button fase3Button;

    private void Start() 
    {
        SetFasesButtonsState();
        if(SceneManager.GetActiveScene().name == "Menu Inicial") ResetCheckPoint();
    }

    public void Jogar()
    {
        JogarFase1();
    }

    public void JogarFase1()
    {
        Debug.Log("Foi fase 1");
        SceneManager.LoadScene("Fase 1");
    }

    public void JogarFase2()
    {
        Debug.Log("Foi fase 2");
        SceneManager.LoadScene("Fase 2");
    }

    public void JogarFase3()
    {
        Debug.Log("Foi fase 3");
        SceneManager.LoadScene("Fase 3");
    }

    public void JogarNovamente()
    {
        GameState.ReloadScene(0);
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene("Menu Inicial");
    }

    public void Fechar()
    {
        Application.Quit();
    }

    private void SetFasesButtonsState()
    {
        var saveManager = new SaveManager();
        var levelsUnlocked = saveManager.LoadGame().unlockLevelsTo;
        if(fase1Button != null) fase1Button.interactable = true;
        if(fase2Button != null) fase2Button.interactable = levelsUnlocked >= 2;
        if(fase3Button != null) fase3Button.interactable = levelsUnlocked >= 3;
    }

    private void ResetCheckPoint()
    {
        var saveManager = new SaveManager();
        var saveData = saveManager.LoadGame();
        saveData = saveManager.ResetCheckPointValue(saveData);
    }
}
