using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    public Image healthBar;
    public GameObject gameOver;

    private void Awake() 
    {
        gameOver.SetActive(false);
    }
}
