using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button continuar;
    [SerializeField] private Button historia;
    [SerializeField] private Button especies;
    [SerializeField] private Button menuInicial;

    void Start()
    {
        continuar.onClick.AddListener(GameState.mainCanvas.ResumeGame);
        historia.onClick.AddListener(GoHistorias);
        especies.onClick.AddListener(GoEspecies);
        menuInicial.onClick.AddListener(GameState.mainCanvas.VoltarMenu);
    }

    private void OnDestroy() 
    {
        continuar.onClick.RemoveListener(GameState.mainCanvas.ResumeGame);
        historia.onClick.RemoveListener(GoHistorias);
        especies.onClick.RemoveListener(GoEspecies);
        menuInicial.onClick.RemoveListener(GameState.mainCanvas.VoltarMenu);
    }

    private void GoHistorias()
    {
        GameState.mainCanvas.book.FlipToPage(4);
    }

    private void GoEspecies()
    {
        GameState.mainCanvas.book.FlipToPage(14);
    }

    public void Restart()
    {
        GameState.RestartStage();
    }
}
