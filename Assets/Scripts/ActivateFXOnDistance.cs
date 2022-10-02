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
        await Task.Delay(Mathf.RoundToInt(updateInterval * 1000));
        UpdateStateAsync();
    }
}
