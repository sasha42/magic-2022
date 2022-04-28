using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public float reachTime = 0.5f;
    public Transform target;
    Vector3 velocity;

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position,target.position,ref velocity, reachTime);
    }
}
