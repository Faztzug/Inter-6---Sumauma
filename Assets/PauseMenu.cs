using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button continuar;
    [SerializeField] private Button opcoes;
    [SerializeField] private Button menuInicial;

    void Start()
    {
        continuar.onClick.AddListener(GameState.mainCanvas.ResumeGame);
        opcoes.onClick.AddListener(GameState.mainCanvas.OpenOptions);
        menuInicial.onClick.AddListener(GameState.mainCanvas.VoltarMenu);
    }

    private void OnDestroy() 
    {
        continuar.onClick.RemoveListener(GameState.mainCanvas.ResumeGame);
        opcoes.onClick.RemoveListener(GameState.mainCanvas.OpenOptions);
        menuInicial.onClick.RemoveListener(GameState.mainCanvas.VoltarMenu);
    }

    public void Restart()
    {
        GameState.saveManager.ResetCheckPointValue(GameState.SaveData);
        GameState.ReloadScene(0f);
    }
}
