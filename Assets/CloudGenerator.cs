using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    // Array of prefab clouds
    public GameObject[] clouds;

    // Control speed of movement
    public float speed;

    // Internal stuff
    GameObject myCloud;
    GameObject[] allClouds;
    int iterationLength;

    // Start is called before the first frame update
    void Start()
    {
        iterationLength = 1000;
        allClouds = new GameObject[iterationLength];

        for (var i = 0; i < iterationLength; i++)
        {
            // Get random cloud from prefabs
            myCloud = clouds[Random.Range(0,clouds.Length)];

            // Generate random starting position around you
            float x = Random.Range(-13.0f, 6.0f);
            float y = Random.Range(-13.0f, 6.0f);
            float z = Random.Range(-13.0f, 6.0f);

            // Instantiate cloud
            allClouds[i] = Instantiate(myCloud, new Vector3(x, y, z), Quaternion.identity) as GameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < iterationLength; i++)
        {
            float x = allClouds[i].transform.position.x + (float)((Time.time * speed)/10000);
            float y = allClouds[i].transform.position.y;
            float z = allClouds[i].transform.position.z;
            allClouds[i].transform.position = new Vector3(x, y, z);
        }
    }
}
