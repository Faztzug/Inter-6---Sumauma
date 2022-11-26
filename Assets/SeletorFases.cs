using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeletorFases : MonoBehaviour
{
    [SerializeField] private Button fase1Button;
    [SerializeField] private Button fase2Button;
    [SerializeField] private Button fase3Button;

    private void Start() 
    {
        SetFasesButtonsState();
    }
    
    public void FlipBookLeft()
    {
        MenuInicial.book.FlipPages(Book.FlipDirection.Left);
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
    

    private void SetFasesButtonsState()
    {
        var saveManager = new SaveManager();
        var levelsUnlocked = saveManager.LoadGame().unlockLevelsTo;
        if(fase1Button != null) fase1Button.interactable = true;
        if(fase2Button != null) fase2Button.interactable = levelsUnlocked >= 2;
        if(fase3Button != null) fase3Button.interactable = levelsUnlocked >= 3;
    }
    
    private void Update()
    {
        if(Input.GetButtonDown("GodMode"))
        {
            Debug.Log("UNLOCK ALL LEVELS!");
            if(fase1Button != null) fase1Button.interactable = true;
            if(fase2Button != null) fase2Button.interactable = true;
            if(fase3Button != null) fase3Button.interactable = true;
        }
    }

}
