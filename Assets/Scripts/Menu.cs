using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene("Fase 1");
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
}
