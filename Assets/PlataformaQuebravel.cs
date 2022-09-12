using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaQuebravel : MonoBehaviour
{
    [SerializeField] private GameObject brokenObject;
    [SerializeField] private float breakTime = 2f;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            Destroy(gameObject, breakTime);
        }
    }

    private void OnDestroy()
    {
        if(brokenObject != null) Instantiate(brokenObject, transform.position, transform.rotation);
    }
}
