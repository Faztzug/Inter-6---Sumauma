using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heliconia : MonoBehaviour
{
    public GameObject Item;
    public GameObject heliconia;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            heliconia.SetActive(true);
            Item.SetActive(false);
        }
    }
}
