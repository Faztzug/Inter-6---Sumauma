using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnCutsceneEnd : MonoBehaviour
{
    void Start()
    {
        GameState.OnCutsceneEnd += Disable;
    }

    private void Disable()
    {
        Debug.Log("DISABLE TIMELINE");
        this.gameObject.SetActive(false);
    }

    private void OnDestroy() 
    {
        GameState.OnCutsceneEnd -= Disable;
    }
}
