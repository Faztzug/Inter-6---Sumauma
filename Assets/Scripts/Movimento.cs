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
        MoveRotation();

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
        Vector3 vertical = Input.GetAxis("Vertical") * transform.forward;
        if(Input.GetAxis("Vertical") < 0) vertical = Input.GetAxis("Vertical") * transform.forward * backWardsMultiplier;
        Vector3 rawHorizontal = Input.GetAxis("Horizontal") * cam.transform.right;
        Vector3 horizontal = rawHorizontal * strafeMultiplier;

        if(controller.isGrounded)
        {
            gravityAcceleration = 0f;
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
            gravityAcceleration -= gravity * Time.deltaTime;
        }

        Vector3 movement = (vertical + horizontal) * Time.deltaTime;
        if(Input.GetButton("Sprint")) currentSpeed += runAccelaration * Time.deltaTime;
        else currentSpeed -= runAccelaration * Time.deltaTime;

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
}