using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    public Image healthBar;
    public GameObject cimaAtivado;
    public GameObject cimaDesativado;
    public GameObject baixoAtivado;
    public GameObject baixoDesativado;


    private void Start()
    {
        cimaAtivado.SetActive(false);
        cimaDesativado.SetActive(true);
        baixoAtivado.SetActive(false);
        baixoDesativado.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            cimaDesativado.SetActive(false);
            cimaAtivado.SetActive(true);
            baixoDesativado.SetActive(false);
            baixoAtivado.SetActive(true);
        }
    }
}
