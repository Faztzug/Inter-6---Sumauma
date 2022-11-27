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
        this.transform.localRotation = Quaternion.identity;
        Debug.Log("RESET CUTSCENE TRANSFORM " + this.transform.localRotation.ToString());
    }

    private void OnDestroy() 
    {
        GameState.OnCutsceneEnd -= Reset;
    }
}
