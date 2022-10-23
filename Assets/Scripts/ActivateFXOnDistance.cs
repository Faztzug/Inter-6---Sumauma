using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ActivateFXOnDistance : MonoBehaviour
{
    [SerializeField] private float distanceToActive;
    [SerializeField] private float updateInterval;

    private void Start() 
    {
        UpdateStateAsync();
    }

    private async void UpdateStateAsync()
    {
        if(this == null || GameState.PlayerTransform == null) return;
        var distance = Vector3.Distance(this.transform.position, GameState.PlayerTransform.position);
        gameObject.SetActive(distance < distanceToActive);
        var delayRNG = Random.Range(updateInterval * 1f, updateInterval * 1.25f);
        await Task.Delay(Mathf.RoundToInt(delayRNG * 1000));
        UpdateStateAsync();
    }
}
