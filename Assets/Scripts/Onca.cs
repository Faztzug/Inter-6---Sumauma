using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onca : MonoBehaviour
{
    public GameObject Item;
    public GameObject onca;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onca.SetActive(false);
            Item.SetActive(true);
        }
    }
}

