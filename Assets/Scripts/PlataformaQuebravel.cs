using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlataformaQuebravel : MonoBehaviour
{
    [SerializeField] private GameObject brokenObject;
    [SerializeField] private float breakTime = 2f; //In seconds
    [SerializeField] private bool breakOnDashOnly = true;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if(breakOnDashOnly && !GameState.isPlayerDashing) return;
            if(brokenObject != null) Instantiate(brokenObject, transform.position, transform.rotation);
            Destroy(gameObject, breakTime);
        }
    }

    private async void Break()
    {
        await Task.Delay(Mathf.RoundToInt(breakTime * 1000));
        GetComponent<Collider>().enabled = false;
        if(brokenObject != null) Instantiate(brokenObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
