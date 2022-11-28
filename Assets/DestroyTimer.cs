using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] float time = 10f;
    void Start()
    {
        Destroy(this.gameObject, time);
    }
}
