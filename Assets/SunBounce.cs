using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBounce : MonoBehaviour
{
    public GameObject sun;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.PingPong(Time.time * speed, 1) * (float)0.1 - 3;
        float orig = -2.08f;
        sun.transform.position = new Vector3((float)2.61, orig-y, (float)1.19);
    }
}
