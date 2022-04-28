using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SunInput : MonoBehaviour
{
    public InputAction encoder;
    public float range = 0.2f;
    public float referenceValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        encoder.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        var input = encoder.ReadValue<float>();
        float distance = Mathf.Abs( input-referenceValue);
        GetComponent<LerpPosition>().lerp = Mathf.InverseLerp(range,0,distance);
    }
}
