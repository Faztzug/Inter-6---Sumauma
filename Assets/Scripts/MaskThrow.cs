using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine.VFX;

public class MaskThrow : MonoBehaviour
{
    [SerializeField] private Transform mask;
    private MaskDamage MaskDamage;
    [SerializeField] private float maskMaxGrow = 1.5f;
    private Quaternion maskRotation;
    private Vector3 maskScale;
    private float maskCurScale;
    private Vector3 maskLocalPos;
    private Rigidbody maskRgbd;
    [SerializeField] private float throwMaxSpeed;
    private float throwCurSpeed;
    [SerializeField] private float throwAccelaration;
    [SerializeField] private float throwDuration;
    private float throwTimer;
    private Vector3 throwDirection;
    private bool onThrow;
    [SerializeField] private TrailRenderer[] trails;
    [SerializeField] private VisualEffect trailVFX;
    private float trailTime;
    private void Start() 
    {
        maskLocalPos = mask.localPosition;
        maskRgbd = mask.GetComponent<Rigidbody>();
        MaskDamage = mask.GetComponent<MaskDamage>();
        maskScale = mask.localScale;
        maskCurScale = 1f;
        maskRotation = mask.localRotation;
        foreach (var trail in trails)
        {
            trail.emitting = false;
            trailTime = trail.time;
        }
        trailVFX.Stop();
    }

    private void Update() 
    {
        if(Input.GetButtonDown("Mask") && !onThrow && !GameState.IsPlayerDead && !GameState.isGamePaused)
        {
            onThrow = true;
            mask.parent = null;
            throwDirection = transform.forward;
            maskRgbd.constraints = RigidbodyConstraints.FreezeRotationX;
            throwTimer = 0;
            maskCurScale = 1f;
            throwCurSpeed = throwAccelaration / 2f;
            trailVFX.Play();
        }

        if(onThrow)
        {
            foreach (var trail in trails)
            {
                trail.time = trailTime;
                trail.emitting = true;
            }
            
            throwTimer += 1 * Time.deltaTime;
            if(throwTimer < throwDuration) OnThrowing();
            else OnComingBack();
        }
    }

    private void OnThrowing()
    {
        throwCurSpeed += throwAccelaration * Time.deltaTime;
        if(throwCurSpeed > throwMaxSpeed) throwCurSpeed = throwMaxSpeed;
        maskRgbd.velocity = throwDirection * throwCurSpeed;
        maskCurScale = Mathf.Lerp(maskCurScale, maskMaxGrow, 1 * Time.deltaTime);
        mask.localScale = maskScale * maskCurScale;
    }
    private void OnComingBack()
    {
        var vectorDistance = transform.position - mask.position;
        throwDirection = vectorDistance.normalized;
        throwCurSpeed += throwAccelaration * Time.deltaTime;
        if(throwCurSpeed > throwMaxSpeed) throwCurSpeed = throwMaxSpeed;
        maskRgbd.velocity = Vector3.Lerp(maskRgbd.velocity, throwDirection * throwCurSpeed, 2f * Time.deltaTime);
        maskCurScale = Mathf.Lerp(maskCurScale, 1f, 1f * Time.deltaTime);
        mask.localScale = maskScale * maskCurScale;
        var distance = Vector3.Distance(mask.position, transform.position);
        if(distance < 1.5f) OnReatach();
    }

    private void OnReatach()
    {
        onThrow = false;
        mask.parent = this.transform;
        mask.localPosition = maskLocalPos;
        mask.localRotation = new Quaternion(0,0,0,0);
        maskRgbd.constraints = RigidbodyConstraints.FreezeAll;
        mask.localScale = maskScale;
        mask.localRotation = maskRotation;
        foreach (var trail in trails) trail.DOTime(0, 0.5f).OnComplete(() => EndTrail(trail));
    }

    private void EndTrail(TrailRenderer trail)
    {
        trail.emitting = false;
        trailVFX.Stop();
    }
}
