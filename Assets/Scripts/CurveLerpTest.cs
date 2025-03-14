using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveLerpTest : MonoBehaviour
{
    public GameObject mover;
    public GameObject start;
    public GameObject end;
    public float speed = 1f;
    public AnimationCurve animCurve;
    public ParticleSystem.MinMaxCurve testCurve;

    private float lerp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //mover.transform.position = Vector3.Lerp(start.transform.position, end.transform.position, animCurve.Evaluate(lerp)); //testCurve.Evaluate(lerp)
        //lerp += Time.deltaTime * speed;
        //if (lerp >= 1) lerp = 0;

        lerp += Time.deltaTime * speed;
        if (lerp >= 1) lerp = 0; 
        //print(Mathf.Sin(lerp));
        mover.transform.position = Vector3.Lerp(start.transform.position, end.transform.position, Mathf.Sin(lerp) / 2 + 0.5f);
    }
}
