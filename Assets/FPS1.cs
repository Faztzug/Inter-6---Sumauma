using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS1 : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();

        InvokeRepeating(nameof(CalcularFPS), 0, 1f);
    }

    // Update is called once per frame
    private void CalcularFPS()
    {
        textMesh.text = (1f / Time.deltaTime).ToString("FPS 00");
    }
}
