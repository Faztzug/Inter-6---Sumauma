using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspeciePagina : MonoBehaviour
{
    [SerializeField] Colectables especie;
    [SerializeField] GameObject imageObj;
    [SerializeField] ColectableType tipoDeColetavel;
    [SerializeField] GameObject tituloObj;
    [SerializeField] GameObject textoObj;
    bool coletado;

    void Start()
    {
        coletado = GameState.GetColectableSaveState(especie);
        imageObj.SetActive(true);
        tituloObj.SetActive(coletado);
        textoObj.SetActive(coletado);
    }
}

public class CutscenePagina : MonoBehaviour
{
    [SerializeField] int levelUnlock;

    void Start()
    {
        var level = GameState.SaveData.unlockLevelsTo;
        this.gameObject.SetActive(levelUnlock >= level);
    }
}
