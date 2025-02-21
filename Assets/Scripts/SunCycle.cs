using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCycle : MonoBehaviour
{
    public float stepDelay = 0.5f;
    public float stepSize = 1f;
    private Vector3 startRot;
    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.rotation.eulerAngles;
        StartCoroutine(RotateCycle());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RotateCycle()
    {
        yield return new WaitForSeconds(stepDelay);
        startRot.x += stepSize;
        transform.eulerAngles = startRot;
        StartCoroutine(RotateCycle());
    }
}
