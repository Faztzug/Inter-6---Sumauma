using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CutscenePagina : MonoBehaviour
{
    [SerializeField] int levelUnlock = 1;

    void Start()
    {
        var level = GameState.SaveData.unlockLevelsTo;
        this.gameObject.SetActive(levelUnlock <= level);
    }
}
