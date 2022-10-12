using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;

public class MaskThrow : MonoBehaviour
{
    [SerializeField] private Transform mask;
    private MaskDamage MaskDamage;
    [SerializeField] private float maskMaxGrow = 1.5f;
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
    private TrailRenderer trail;
    private float trailTime;
    private void Start() 
    {
        maskLocalPos = mask.localPosition;
        maskRgbd = mask.GetComponent<Rigidbody>();
        MaskDamage = mask.GetComponent<MaskDamage>();
        maskScale = mask.localScale;
        maskCurScale = 1f;
        trail = mask.GetComponent<TrailRenderer>();
        trail.emitting = false;
        trailTime = trail.time;
    }

    private void Update() 
    {
        if(Input.GetButtonDown("Mask") && !onThrow)
        {
            onThrow = true;
            mask.parent = null;
            throwDirection = mask.forward;
            maskRgbd.constraints = RigidbodyConstraints.FreezeRotationX;
            throwTimer = 0;
            maskCurScale = 1f;
            throwCurSpeed = throwAccelaration / 2f;
        }

        if(onThrow)
        {
            trail.time = trailTime;
            trail.emitting = true;
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
        trail.DOTime(0, 0.5f).OnComplete(() => trail.emitting = false);
    }
}
