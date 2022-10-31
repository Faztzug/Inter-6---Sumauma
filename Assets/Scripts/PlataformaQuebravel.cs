using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlataformaQuebravel : MonoBehaviour
{
    [SerializeField] private GameObject brokenObject;
    [SerializeField] private float breakTime = 2f; //In seconds
    [SerializeField] private bool breakOnDashOnly = true;
    public Sound breakSound;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if(breakOnDashOnly && !GameState.isPlayerDashing) return;
            Break();
        }
    }

    private async void Break()
    {
        await Task.Delay(Mathf.RoundToInt(breakTime * 1000));
        if(this == null) return;
        GetComponent<Collider>().enabled = false;
        if(breakSound.clip != null) GameState.InstantiateSound(breakSound, transform.position);
        if(brokenObject != null) Instantiate(brokenObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
