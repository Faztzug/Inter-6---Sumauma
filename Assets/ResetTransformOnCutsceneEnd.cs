using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTransformOnCutsceneEnd : MonoBehaviour
{
    void Start()
    {
        GameState.OnCutsceneEnd += Reset;
    }

    private void Reset()
    {
        Debug.Log("RESET CUTSCENE TRANSFORM");
        this.transform.localRotation = Quaternion.identity;
    }

    private void OnDestroy() 
    {
        GameState.OnCutsceneEnd -= Reset;
    }
}
