using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    private void Start() 
    {
        if(SceneManager.GetActiveScene().name == "Menu Inicial") ResetCheckPoint();
    }

    public void Jogar()
    {
        SceneManager.LoadScene("Fase 1");
    }

    public void NovoJogo()
    {
        var save = new SaveManager();
        save.ResetData();
        SceneManager.LoadScene("Fase 1");
    }

    public void FlipBookRight()
    {
        MenuInicial.book.FlipPages(Book.FlipDirection.Right);
    }
    public void FlipBookInstructions()
    {
        MenuInicial.book.FlipToPage(4);
    }
    public void FlipBookBack()
    {
        MenuInicial.book.FlipToPage(0);
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

    private void ResetCheckPoint()
    {
        var saveManager = new SaveManager();
        var saveData = saveManager.LoadGame();
        saveData = saveManager.ResetCheckPointValue(saveData);
    }
}
