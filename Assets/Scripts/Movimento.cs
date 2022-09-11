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
    public float dashCooldownTime;
    private float dashCooldownState;
    private float boing;

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
        Application.targetFrameRate = 60;
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

    private void MoveInput()
    {
        var camRot = cam.transform.rotation;
        var newCamRot = new Quaternion(0, camRot.y, 0, camRot.w);
        cam.transform.rotation = newCamRot;
        Vector3 vertical = Input.GetAxis("Vertical") * cam.transform.forward;
        Vector3 horizontal = Input.GetAxis("Horizontal") * cam.transform.right;

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
            if (Input.GetButtonDown("Jump") && podeDoubleJump && gravityAcceleration < jumpForce * doubleJump)
            {
                gravityAcceleration = jumpForce * doubleJump;
                podeDoubleJump = false;
            }
            gravityAcceleration -= gravity * Time.deltaTime;
        }

        DashUpdate();

        Vector3 movement = (vertical + horizontal).normalized;
        rotateObj.position = transform.position + movement;
        if(rotateObj.localPosition != Vector3.zero) 
        {
            transform.LookAt(rotateObj);
        }
        cam.transform.rotation = camRot;
        rotateObj.position = transform.position + movement;
        if(Input.GetButton("Sprint")) currentSpeed += runAccelaration * Time.deltaTime;
        else currentSpeed -= runAccelaration * Time.deltaTime;

        movement = movement * Time.deltaTime;

        if(currentSpeed < speed) currentSpeed = speed;
        else if(currentSpeed > runSpeed) currentSpeed = runSpeed;

        movement = movement * currentSpeed;

        if(boing > 0)
        {
            gravityAcceleration = boing;
            boing = 0;
        }
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

    private void DashUpdate()
    {
        if (Input.GetButtonDown("Dash"))
        {
            StartCoroutine(Dash());
        }
        dashCooldownState -= 1f * Time.deltaTime;
    }

    public void ImpulseJump(float force)
    {
        boing = force;
        Debug.Log("BOING");
    }

    IEnumerator Dash()
    {
        if(dashCooldownState > 0f) Debug.Log("Null Dash");
        if(dashCooldownState > 0f) yield return null;
        else
        {
            dashCooldownState = dashCooldownTime;
            float startTime = Time.time;
            Debug.Log("Dashou");

            while(Time.time < startTime + dashTime)
            {
                controller.Move(transform.forward * dashSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }
}