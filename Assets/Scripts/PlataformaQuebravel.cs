using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlataformaQuebravel : MonoBehaviour
{
    [SerializeField] private GameObject brokenObject;
    [SerializeField] private float breakTime = 2f; //In seconds
    [SerializeField] private bool breakOnTouch = true;
    [SerializeField] private bool breakOnDash = true;
    [SerializeField] private bool breakOnMask = true;
    public Sound breakSound;
    private bool alreadyBroken;

    void OnTriggerEnter(Collider collider)
    {
        BreakCheck(collider);
    }

    private void OnTriggerStay(Collider collider) 
    {
        if(this != null && this.gameObject) BreakCheck(collider);
    }

    private void BreakCheck(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if(breakOnTouch) Break();
            if(breakOnDash && GameState.isPlayerDashing) Break();
        }
        else if (collider.gameObject.CompareTag("Mask"))
        {
            if(breakOnMask && collider.transform.parent == null) 
            {
                Break();
            }
        }
    }

    private async void Break()
    {
        if(alreadyBroken) return;
        alreadyBroken = true;
        await Task.Delay(Mathf.RoundToInt(breakTime * 1000));
        if(this == null) return;
        GetComponent<Collider>().enabled = false;
        if(breakSound.clip != null) GameState.InstantiateSound(breakSound, transform.position);
        if(brokenObject != null) Instantiate(brokenObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
