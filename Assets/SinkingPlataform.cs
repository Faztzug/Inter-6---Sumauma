using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SinkingPlataform : MonoBehaviour
{
    [SerializeField] private bool parentPlayer = false;
    [SerializeField] private Rigidbody rgbd;
    [SerializeField] private float dragScaleFactor = 1f;
    private float originalYPos;
    private Tween floatingBack;
    private Transform player;
    public Sound sinkingSound;
    private bool sinking;

    private void Start() 
    {
        originalYPos = transform.localPosition.y;
        rgbd.drag = rgbd.drag * dragScaleFactor;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player")) Debug.Log("!SIM");
        if(collider.CompareTag("Player") && parentPlayer && !sinking)
        {
            floatingBack?.Kill();
            player = collider.transform.parent = this.transform.parent;
            sinking = true;
            rgbd.useGravity = true;
        }
    }

    private void OnTriggerExit(Collider collider) 
    {
        if(collider.CompareTag("Player"))
        {
            collider.transform.parent = null;
            player = null;
            sinking = false;
            rgbd.useGravity = false;
            floatingBack = transform.DOLocalMoveY(originalYPos, 4f).SetEase(Ease.OutElastic);
        }
    }
}
