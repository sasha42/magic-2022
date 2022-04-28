using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPosition : MonoBehaviour
{
    public Transform obj1;
    public Transform obj2;

    [Range(0,1)]
    public float lerp;

    void Update()
    {
        transform.position = Vector3.Lerp(obj1.position,obj2.position,lerp);
    }
}
