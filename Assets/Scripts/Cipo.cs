using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cipo : MonoBehaviour
{
    [SerializeField] private bool comecarDaEsquerda = true;
    [SerializeField] private Transform rootVine;
    [SerializeField] private float rotationSpeed =  1f;
    [SerializeField] private float maxRotation = 0.5f;
    [SerializeField] private float damage = 30f;
    private bool positiveRot = true;

    void Start()
    {
        var startRot = comecarDaEsquerda ? -maxRotation : maxRotation;
        var rot = rootVine.localRotation;
        rot.z = startRot;
        rootVine.localRotation = rot;
        foreach (var spike in GetComponentsInChildren<Spiky>())
        {
            spike.damageByTouch = damage;
        }
    }

    private void Update() 
    {
        var rot = rootVine.localRotation;
        if(rot.z > maxRotation) positiveRot = false;
        if(rot.z < -maxRotation) positiveRot = true;
        var rotSpeed = positiveRot ? rotationSpeed : -rotationSpeed; 
        rot.z += (rotSpeed) * Time.deltaTime;
        rootVine.localRotation = rot;
    }

    private void OnValidate() 
    {
        if(damage > 0) damage = -damage;
    }

}
