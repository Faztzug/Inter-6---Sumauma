using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimento : MonoBehaviour
{
    [Header("Character Values")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float runAccelaration = 3f;
    private float currentSpeed;
    [SerializeField] [Range(0.5f,1f)] private float backWardsMultiplier = 0.5f;
    [SerializeField] [Range(0.5f,1f)] private float strafeMultiplier = 0.9f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float doubleJump = 0.5f;
    private bool podeDoubleJump = false;
    public float dashSpeed;
    public float dashTime;

    [Header("Needed References")]
    [SerializeField] private Transform rotateObj;
    private CharacterController controller;
    private Camera cam;
    //private Animator anim;

    [Header("Gravity Values")]
    [SerializeField] private float gravity = 1f;
    private float gravityAcceleration;
    
    // [Header("Audio")]
    // [SerializeField] private AudioSource audioSource;
    // [SerializeField] private AudioClip passosClip;

    private bool paused;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        //anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!paused)
        {
            Movement();
            Animations();
        }
    }

    private void Movement()
    {
        MoveInput();
    }

    private void Animations()
    {
        
    }

    private void MoveRotation()
    {
        var camRotation = cam.transform.rotation;
        var objRotation = transform.rotation;
        Vector3 setRotation = new Vector3(objRotation.eulerAngles.x, camRotation.eulerAngles.y, objRotation.eulerAngles.z);
        transform.eulerAngles = setRotation;
    }

    private void MoveInput()
    {
        var camRot = cam.transform.rotation;
        var newCamRot = new Vector3(0, camRot.y, 0);
        cam.transform.eulerAngles = newCamRot;
        Vector3 vertical = Input.GetAxis("Vertical") * cam.transform.forward;
        Vector3 horizontal = Input.GetAxis("Horizontal") * cam.transform.right;
        cam.transform.rotation = camRot;

        if (controller.isGrounded)
        {
            gravityAcceleration = 0f;
            podeDoubleJump = true;
            //anim.SetBool("isJumping", false);

            if (Input.GetButtonDown("Jump"))
            {
                gravityAcceleration = jumpForce;
                //anim.SetBool("isJumping", true);
            }
            else gravityAcceleration = -gravity * 10f * Time.deltaTime;

        }
        else
        {
            if (Input.GetButtonDown("Jump") && podeDoubleJump)
            {
                gravityAcceleration = jumpForce * doubleJump;
                podeDoubleJump = false;
            }
            gravityAcceleration -= gravity * Time.deltaTime;
        }

        if (Input.GetButtonDown("Dash"))
        {
            Debug.Log("Dashou");
            StartCoroutine(Dash());
        }

        Vector3 movement = (vertical + horizontal).normalized;
        rotateObj.localPosition = movement;
        if(rotateObj.localPosition != Vector3.zero) 
        {
            MoveRotation();
            transform.LookAt(rotateObj);
        }
        if(Input.GetButton("Sprint")) currentSpeed += runAccelaration * Time.deltaTime;
        else currentSpeed -= runAccelaration * Time.deltaTime;

        movement = movement * Time.deltaTime;

        if(currentSpeed < speed) currentSpeed = speed;
        else if(currentSpeed > runSpeed) currentSpeed = runSpeed;

        movement = movement * currentSpeed;

        movement.y = gravityAcceleration * Time.deltaTime * speed;
        
        controller.Move(movement);
        
        var velocitylAbs = Mathf.Abs(vertical.magnitude) + Mathf.Abs(horizontal.magnitude);
        //anim.SetFloat("Movement", velocitylAbs);

        //anim.SetFloat("Velocidade", Mathf.Abs((vertical.magnitude * currentSpeed) / runSpeed));

        //if(Input.GetAxis("Horizontal") >= 0) anim.SetFloat("Strafe", (horizontal.magnitude * currentSpeed) / runSpeed);
        //else if(Input.GetAxis("Horizontal") < 0) anim.SetFloat("Strafe", (-horizontal.magnitude * currentSpeed) / runSpeed);

        //Audio
        // if((velocitylAbs > 0.1) && controller.isGrounded)
        // {
        //     if(audioSource.isPlaying == false)
        //     {
        //         audioSource.PlayOneShot(passosClip);
        //     }
        // }
        // else
        // {
        //     audioSource.Stop();
        // }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            controller.Move(transform.forward * dashSpeed * Time.deltaTime);

            yield return null;
        }
    }
}