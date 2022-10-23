using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Foward,
    Right,
    Around,
}

public class Rola : MonoBehaviour
{
    //https://youtu.be/dQw4w9WgXcQ
    [SerializeField] private float MoveSpeed;
    private Vector3 MoveSpeedVector;
    [SerializeField] private float RollSpeed;
    [SerializeField] private Direction RollingDirection;
    [SerializeField] private bool startRoolingOnTrigger;
    private bool moving;
    private Vector3 RollSpeedVector;
    private Rigidbody rgbd;
    private Vector3 fowardDirection;
    [SerializeField] private int addForcePower = 50;

    void Start()
    {
        fowardDirection = transform.forward;
        if(!startRoolingOnTrigger) StartRooling();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) {StartRooling(); }
    }

    private void StartRooling()
    {
        Debug.Log("ROLLINGGG  " + name);
        rgbd = GetComponent<Rigidbody>();
        MoveSpeedVector = fowardDirection * MoveSpeed;
        if(RollingDirection == Direction.Foward) 
        {
            rgbd.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            RollSpeedVector = new Vector3(RollSpeed, 0, 0);
        }
        if(RollingDirection == Direction.Right)
        {
            rgbd.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
            RollSpeedVector = new Vector3(0, 0, RollSpeed);
        }
        if(RollingDirection == Direction.Around)
        {
            rgbd.constraints =  RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            RollSpeedVector = new Vector3(0, RollSpeed, 0);
        }
        moving = true;
        // rgbd.angularDrag = 0;
        // rgbd.drag = 0;
        // rgbd.angularVelocity = RollSpeedVector;
        // rgbd.velocity = MoveSpeedVector;
    }

    private void FixedUpdate()
    {
        if(!moving) return;
        rgbd.AddForce(MoveSpeedVector * addForcePower, ForceMode.Impulse);
        rgbd.AddTorque(RollSpeedVector * addForcePower, ForceMode.Impulse);
    }
}
