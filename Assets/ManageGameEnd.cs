using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGameEnd : MonoBehaviour
{
    private bool isOnBook;
    private void Start() 
    {
        GameState.OnCutsceneEnd += EndHandler;
    }
    private void OnDestroy() 
    {
        GameState.OnCutsceneEnd -= EndHandler;
    }

    private void Update() 
    {
        if(isOnBook && Time.timeScale == 1) GameState.LoadScene("Menu Inicial");
    }

    public void EndHandler()
    {
        StartCoroutine(EndPages());
    }

    IEnumerator EndPages()
    {
        var canvas = GameState.mainCanvas;
        canvas.PauseGame();
        yield return new WaitForSecondsRealtime(1f);
        canvas.book.FlipToPage(20);
        isOnBook = true;
        yield return new WaitForSecondsRealtime(60f);
        canvas.ResumeGame();
    }
}
