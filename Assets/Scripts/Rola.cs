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
    private Vector3 RollSpeedVector;
    private Rigidbody rgbd;

    void Start()
    {
        rgbd = GetComponent<Rigidbody>();
        MoveSpeedVector = transform.forward * MoveSpeed;
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
            rgbd.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            RollSpeedVector = new Vector3(0, RollSpeed, 0);
        }
    }

    private void Update() 
    {
        rgbd.angularVelocity = RollSpeedVector;
        rgbd.velocity = MoveSpeedVector;
    }
}
