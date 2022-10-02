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

    void Start()
    {
        if(!startRoolingOnTrigger) StartRooling();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) {StartRooling(); 
        Debug.Log("ROLLINGGG");}
    }

    private void StartRooling()
    {
        rgbd = GetComponent<Rigidbody>();
        MoveSpeedVector = transform.forward * MoveSpeed;
        if(RollingDirection == Direction.Foward) 
        {
            rgbd.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            RollSpeedVector = new Vector3(RollSpeed, 0, 0);
        }
        if(RollingDirection == Direction.Right)
        {
            rgbd.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
            RollSpeedVector = new Vector3(0, 0, RollSpeed);
        }
        if(RollingDirection == Direction.Around)
        {
            rgbd.constraints =  RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            RollSpeedVector = new Vector3(0, RollSpeed, 0);
        }
        moving = true;
    }

    private void FixedUpdate()
    {
        if(!moving) return;
        rgbd.angularDrag = 0;
        rgbd.drag = 0;
        rgbd.angularVelocity = RollSpeedVector;
        rgbd.velocity = MoveSpeedVector;
    }
}
