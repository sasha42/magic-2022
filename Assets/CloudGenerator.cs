using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloudGenerator : MonoBehaviour
{
    // Array of prefab clouds
    public GameObject[] clouds;

    // Control speed of movement

    public float constantSpeed;

    public ParticleSystem particles;

    // Bounds of clouds
    public float leftBounds;
    public float rightBounds;
    public float lowerBounds;
    public float topBounds;

    // Bounds of buttons
    public float minButton;
    public float maxButton;

    // Button stuff
    public InputAction button;
    public InputAction encoder;
    private float startScaleY;

    // Motor stuff
    Esp32InputDevice lastInputDevice;
    float lastValue;
	float speed = 0;
    float position;

    // Internal stuff
    GameObject myCloud;
    GameObject[] allClouds;
    int iterationLength;
    float lastVal;

    // Smooth moving around
    int[] lerpStates = {0, 0, 0, 0, 0, 0, 0, 0, 0};

    // Start is called before the first frame update
    void Start()
    {
        // Enable button
        startScaleY = transform.localScale.y;
        button.Enable();
        encoder.Enable();

        // Generate clouds
        iterationLength = 1500;
        allClouds = new GameObject[iterationLength];

        for (var i = 0; i < iterationLength; i++)
        {
            // Get random cloud from prefabs
            myCloud = clouds[Random.Range(0,clouds.Length)];

            // Generate random starting position around you
            float x = Random.Range(leftBounds, rightBounds);
            float y = Random.Range(-5.0f, 6.0f);
            float z = Random.Range(lowerBounds, topBounds);

            // Instantiate cloud
            allClouds[i] = Instantiate(myCloud, new Vector3(x, y, z), Quaternion.identity) as GameObject;
        }

        // Stop particles by default
        particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        // Read rotary encoder value
        float encoderVal = encoder.ReadValue<float>();


        // Compuate delta for movement
        float targetSpeed = (encoderVal - lastVal) / Time.deltaTime;
        speed = Mathf.Lerp(speed,targetSpeed,Time.deltaTime*3);
        position += speed * Time.deltaTime;

        Debug.Log(position);

        // moveVal = Mathf.Lerp(moveVal, lastVal, Time.deltaTime);
        // lastInputDevice.SendMotorSpeed(Mathf.InverseLerp(0,2f, smoothSpeed));

        ////////////
        /*if (moveVal != 0) {
            // Generate lerp between this and last move val
            for (var i = 1; i < 10; i++) {
                float lerpVal = Mathf.Lerp(moveVal, lastVal, (float)((float)1/(float)i));

            }
            // Save to array
       // } else {

        }*/
        ////////////

        // Save current value to calculate delta
        lastVal = encoderVal;


        // Move all the clouds
        for (var i = 0; i < iterationLength; i++)
        {
            // Standard moving like as if there is wind
            float x = allClouds[i].transform.position.x;
            float y = allClouds[i].transform.position.y;
            float z = allClouds[i].transform.position.z;


            // If clouds are out of bounds, move them back
            if (x > rightBounds) {
                // Debug.Log("Out of bounds");
                 x = leftBounds;
            } 

            x += (float)constantSpeed * Time.deltaTime;
            
            // Move clouds based on rotary encoder
            if ( position < maxButton+0.1 && position > minButton-0.1) {
                // Debug.Log(moveVal*10);
                x -= (float)(speed*0.2f);
            }
            // Debug.Log(position);

            allClouds[i].transform.position = new Vector3(x, y, z);
            // allClouds[i].material.SetColor("_Color", Color.red);
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

                // particles.Play();
                // particles.emissionRate = 1500*value;

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
                // particles.Play();
                // particles.emissionRate = 1500*value;
            // } else {
                // lastInputDevice.SendMotorSpeed(0);
                // particles.Stop();
            }

            // Do the rain
             if (encoderVal > maxButton) {
                float value = encoder.ReadValue<float>();
                particles.Play();
                particles.emissionRate = Mathf.Pow(100*value, 2);
            } else if (encoderVal < minButton) {
                float value = Mathf.Abs(encoder.ReadValue<float>());
                particles.Play();
                particles.emissionRate = Mathf.Pow(100*value, 2);
            } else {
                lastInputDevice.SendMotorSpeed(0);
                particles.Stop();
            }
		}
    }
}
