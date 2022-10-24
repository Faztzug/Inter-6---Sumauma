using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
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
}
