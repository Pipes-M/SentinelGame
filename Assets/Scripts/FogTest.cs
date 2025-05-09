using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTest : MonoBehaviour
{
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.fogDensity += Time.deltaTime * speed;
    }
}
