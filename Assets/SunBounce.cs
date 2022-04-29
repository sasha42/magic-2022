using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SunBounce : MonoBehaviour
{
    public GameObject sun;
    public float speed;
    public float sunOffset;
    float origY;

    // Button stuff
    public InputAction encoder;

    // Velocity stuff
    // public float velocity = 1;
    public float reachTime = 5f;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        origY = sun.transform.position.y;
        encoder.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // Read encoder value
        float val = encoder.ReadValue<float>();
        // Debug.Log(val);

        if (val > 0.2) {
            // Debug.Log(val);
            sunOffset = -Mathf.Abs((((float)val-(float)0.2)*10));
        } else {
            sunOffset = -5;
        }

        // Adjust sun
        float yAdjust = Mathf.PingPong(Time.time * speed, 1) * (float)0.1 - 3;
        float orig = 1.76f;

        float x = sun.transform.position.x;
        // float y = sun.transform.position.y;
        float z = sun.transform.position.z;

        // Smooth adjust
        Vector3 newPos = new Vector3(x, origY-yAdjust-sunOffset, z); 
        sun.transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, reachTime);
        // sun.transform.position = Vector3.SmoothDamp(transform.position,target.position,ref velocity, reachTime);

        // sun.transform.position = new Vector3(x, origY-yAdjust-sunOffset, z);
    }
}
