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
