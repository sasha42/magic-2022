using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloudGenerator : MonoBehaviour
{
    // Array of prefab clouds
    public GameObject[] clouds;

    // Control speed of movement
    public float speed;

    // Bounds of clouds
    public float leftBounds;
    public float rightBounds;

    // Bounds of buttons
    public float minButton;
    public float maxButton;

    // Button stuff
    public InputAction button;
    public InputAction encoder;
    private float startScaleY;
    float encoderVal;

    // Motor stuff
    Esp32InputDevice lastInputDevice;
    float lastValue;
	float smoothSpeed = 0;

    // Internal stuff
    GameObject myCloud;
    GameObject[] allClouds;
    int iterationLength;
    float lastVal;

    // Start is called before the first frame update
    void Start()
    {
        // Enable button
        startScaleY = transform.localScale.y;
        button.Enable();
        encoder.Enable();

        // Generate clouds
        iterationLength = 2000;
        allClouds = new GameObject[iterationLength];

        for (var i = 0; i < iterationLength; i++)
        {
            // Get random cloud from prefabs
            myCloud = clouds[Random.Range(0,clouds.Length)];

            // Generate random starting position around you
            float x = Random.Range(leftBounds, rightBounds);
            float y = Random.Range(-13.0f, 6.0f);
            float z = Random.Range(-13.0f, 6.0f);

            // Instantiate cloud
            allClouds[i] = Instantiate(myCloud, new Vector3(x, y, z), Quaternion.identity) as GameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Read rotary encoder value
        encoderVal = encoder.ReadValue<float>();


        // Compuate delta for movement
        float moveVal = (encoderVal - lastVal);

        // Save current value to calculate delta
        lastVal = encoderVal;

        // Move all the clouds
        for (var i = 0; i < iterationLength; i++)
        {
            // Standard moving like as if there is wind
            float x = allClouds[i].transform.position.x + (float)((Time.time * speed)/10000);
            float y = allClouds[i].transform.position.y;
            float z = allClouds[i].transform.position.z;

            // If clouds are out of bounds, move them back
            if (x > rightBounds) {
                // Debug.Log("Out of bounds");
                x = leftBounds;
            } 

            // Move clouds based on rotary encoder
            if (moveVal != 0 && encoderVal < maxButton+0.1 && encoderVal > minButton-0.1) {
                x = allClouds[i].transform.position.x - (float)(moveVal*10);
            }

            allClouds[i].transform.position = new Vector3(x, y, z);
        }
        
		if (encoder.activeControl != null)
		{
			lastInputDevice = encoder.activeControl.device as Esp32InputDevice;
		}

		if (lastInputDevice != null) 
		{
            // Vibrate if out of bounds
            if (encoderVal > maxButton-0.3) {
                encoderVal = lastVal;
                float value = encoder.ReadValue<float>();

                float adjustedValue = value * 1.2f;
                // float speed = Mathf.Abs(adjustedValue - lastValue);
                // smoothSpeed = Mathf.Lerp(smoothSpeed, speed, Time.deltaTime * 12);
                // Debug.Log(smoothSpeed);
                // lastInputDevice.SendMotorSpeed(Mathf.InverseLerp(0,2f, smoothSpeed));
                lastInputDevice.SendMotorSpeed(adjustedValue);
                // lastValue = adjustedValue;
            } else if (encoderVal < minButton+0.3) {
                encoderVal = lastVal;
                float value = Mathf.Abs(encoder.ReadValue<float>());
                // float speed = Mathf.Abs(value - lastValue) / Time.deltaTime;
                // smoothSpeed = Mathf.Lerp(smoothSpeed, speed, Time.deltaTime * 12);
                float adjustedValue = value * 1.2f;
                // float speed = Mathf.Abs(adjustedValue - lastValue);
                // smoothSpeed = Mathf.Lerp(smoothSpeed, speed, Time.deltaTime * 12);
                // Debug.Log(smoothSpeed);
                // lastInputDevice.SendMotorSpeed(Mathf.InverseLerp(0,2f, smoothSpeed));
                lastInputDevice.SendMotorSpeed(adjustedValue);
                // lastInputDevice.SendMotorSpeed(0.5f);
                // lastValue = value;
            } else {
                lastInputDevice.SendMotorSpeed(0);
            }
		}
    }
}
