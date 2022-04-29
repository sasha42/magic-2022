using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LookAt : MonoBehaviour
{
    public GameObject target;
    
    void Update()
    {
        Vector3 direction = target.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction,Vector3.up) * Quaternion.Euler(0,180,0);
    }
}
