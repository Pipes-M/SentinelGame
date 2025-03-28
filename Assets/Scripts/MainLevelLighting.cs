using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLevelLighting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.ambientIntensity = 1f;
        RenderSettings.reflectionIntensity = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
