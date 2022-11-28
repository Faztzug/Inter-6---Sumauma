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
    public bool allowDoubleJump = true;
    [SerializeField] private float doubleJump = 0.5f;
    public bool podeDoubleJump = false;
    [SerializeField] private Transform playerMiddle;
    [SerializeField] private float knockBackForce = 5f;
    [SerializeField] private float knockUpForce = 2f;
    [SerializeField] private float knockBackTime = 0.5f;
    private float knockBackCounterTime;
    public bool onKnockBack => knockBackCounterTime > 0;
    private Vector3 knockBackImpulse = Vector3.zero;
    private float knockX;
    private float knockZ;
    public float dashSpeed;
    public float dashTime;
    public float dashCooldownTime;
    private float dashCooldownState;
    private float boing;

    [Header("Needed References")]
    [SerializeField] private Transform rotateObj;
    private CharacterController controller;
    private Camera cam;
    private Animator anim;

    [Header("Gravity Values")]
    [SerializeField] private float gravity = 1f;
    private float gravityAcceleration;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] Sound dashSound;
    [SerializeField] Sound jumpSound;
    [SerializeField] Sound jumpVoice;
    [SerializeField] Sound doubleJumpVoice;
    // [SerializeField] private AudioClip passosClip;

    private Vector3 checkpointPosition;
    private int tryGoCheckPointTimes = 30;

    private bool paused;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GameState.MainCamera;
        Application.targetFrameRate = 60;
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!GameState.isGamePaused)
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
        if(checkpointPosition != Vector3.zero && tryGoCheckPointTimes > 0) 
        {
            Debug.Log("Going to BRAZIL " + checkpointPosition.ToString());
            transform.position = Vector3.zero;
            //controller.Move(checkpointPosition);
            transform.position = checkpointPosition;
            tryGoCheckPointTimes--;
            if(tryGoCheckPointTimes <= 0) checkpointPosition = Vector3.zero;
            return;
        }
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

            if (Input.GetButtonDown("Jump") && !GameState.IsPlayerDead && !onKnockBack && !GameState.isOnCutscene)
            {
                gravityAcceleration = jumpForce;
                jumpSound.PlayOn(audioSource);
                jumpVoice.PlayOn(audioSource2);
                anim.SetTrigger("Jump");
            }
            else gravityAcceleration = -gravity * 10f * Time.deltaTime;

        }
        else
        {
            if (Input.GetButtonDown("Jump") && podeDoubleJump && allowDoubleJump 
            && gravityAcceleration < jumpForce * doubleJump && !GameState.IsPlayerDead
            && !onKnockBack && !GameState.isOnCutscene)
            {
                gravityAcceleration = jumpForce * doubleJump;
                podeDoubleJump = false;
                jumpSound.PlayOn(audioSource);
                doubleJumpVoice.PlayOn(audioSource2);
                anim.SetTrigger("Jump");
            }
            gravityAcceleration -= gravity * Time.deltaTime;
        }

        DashUpdate();

        if(onKnockBack || GameState.isOnCutscene)
        {
            vertical = Vector3.zero;
            horizontal = Vector3.zero;
        }
        knockBackCounterTime -=  1 * Time.deltaTime;

        Vector3 movement = (vertical + horizontal).normalized;
        if(GameState.IsPlayerDead) movement = Vector3.zero;
        rotateObj.position = transform.position + movement;
        if(rotateObj.localPosition != Vector3.zero) 
        {
            transform.LookAt(rotateObj);
        }
        cam.transform.rotation = camRot;
        rotateObj.position = transform.position + movement;
        if(Input.GetButton("Sprint") || Input.GetAxisRaw("Sprint") != 0) currentSpeed += runAccelaration * Time.deltaTime;
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

        if(knockBackImpulse != Vector3.zero)
        {
            knockX = knockBackImpulse.x;
            gravityAcceleration = knockBackImpulse.y;
            knockZ = knockBackImpulse.z;
            knockBackImpulse = Vector3.zero;
        }
        if(knockX > 0) movement.x = knockX;
        if(knockZ > 0) movement.z = knockZ;
        knockX -= 1 * Time.deltaTime;
        knockZ -= 1 * Time.deltaTime;


        movement.y = gravityAcceleration * Time.deltaTime * speed;
        
        if(!GameState.isGamePaused) controller.Move(movement);
        
        var velocitylAbs = Mathf.Abs(vertical.magnitude) + Mathf.Abs(horizontal.magnitude);
        anim.SetFloat("Movement", velocitylAbs);

        if (velocitylAbs > 0.1f && (Input.GetButton("Sprint") || Input.GetAxisRaw("Sprint") != 0))
        {
            anim.SetBool("Correndo", true);
        }
        else anim.SetBool("Correndo", false);
    }

    private void DashUpdate()
    {
        if (Input.GetButtonDown("Dash") && !GameState.IsPlayerDead && !onKnockBack && !GameState.isOnCutscene)
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

    public void KnockBack(Vector3 enemyPos, float modifyForce = 0f)
    {
        knockBackCounterTime = knockBackTime;
        var vectorDistance =  playerMiddle.position - enemyPos;
        var knockBackDir = vectorDistance.normalized;
        var curKnockForce = knockBackForce + modifyForce;
        knockBackDir.y += knockUpForce;
        knockBackImpulse = knockBackDir * curKnockForce;
    }

    public void ResetKnockBackTimer()
    {
        knockBackCounterTime = 0;
    }

    public void GoToCheckPoint(Vector3 position)
    {
        checkpointPosition = position;
    }

    IEnumerator Dash()
    {
        if(dashCooldownState > 0f) Debug.Log("Dash In Cooldown");
        if(dashCooldownState > 0f) yield return null;
        else
        {
            anim.SetTrigger("Dash");
            dashSound.PlayOn(audioSource);
            dashCooldownState = dashCooldownTime;
            float startTime = Time.time;
            GameState.isPlayerDashing = true;

            while(Time.time < startTime + dashTime)
            {
                controller.Move(transform.forward * dashSpeed * Time.deltaTime);

                yield return null;
            }
            
            
            GameState.isPlayerDashing = false;
        }
    }
}