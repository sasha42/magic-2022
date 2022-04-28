using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAnim : MonoBehaviour
{
    public Material[] material;
    Material m_Material;
    Renderer rend;
    private float nextActionTime = 0.0f;
    public float period = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Material from the Renderer of the GameObject
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
        // m_Material = material[0];
        // print(m_Material);
        // print("Materials " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length);
    }

    // Update is called once per frame
    void Update()
    {
        // m_Material = material[1];
        if (Time.time > nextActionTime ) {
            nextActionTime += period;

            if (rend.sharedMaterial == material[1]) {
                rend.sharedMaterial = material[0];
                period = 0.5f;
            } else {
                rend.sharedMaterial = material[1];
                period = 3.5f;
            }
            // rend.sharedMaterial = 
                // rend.sharedMaterial == material[1] ? material[0] : material[1];
            // rend.sharedMaterial = material[1];
        }
        
    }
}
