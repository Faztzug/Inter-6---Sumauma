using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaQuebravel : MonoBehaviour
{

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            Destroy(gameObject, 2);
        }
    }
}
