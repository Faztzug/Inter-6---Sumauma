using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void TentarNovamente()
    {
        GameState.ReloadScene(5f);
    }

    public void MenuIncial()
    {
        SceneManager.LoadScene("Menu Inicial");
    }
}
