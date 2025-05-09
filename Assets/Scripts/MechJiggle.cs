using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MechJiggle : MonoBehaviour
{
    public GameObject mechPelivs;
    public GameObject mechPelivsParent;
    public float leanSide; //negative = lean left. positive = lean right
    public float height;
    public float stepHeightStrength = 1f;

    public float jiggleDuration = 1f;
    public float jiggleFreq = 1f;
    public float jiggleIntensity = 1f;

    private float jiggleAngle;
    private float jiggleTimer = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        JiggleUpdate();
        mechPelivs.transform.localRotation = Quaternion.Euler(leanSide + jiggleAngle - 90f, 90f, 0f);


        mechPelivs.transform.localPosition = new Vector3(0, height, 0);

        
    }

    public void Jiggle()
    {
        jiggleTimer = 0f;
        //print("called");
    }

    void JiggleUpdate()
    {
        if (jiggleTimer < jiggleDuration)
        {
            jiggleTimer += Time.deltaTime;
            jiggleAngle = Mathf.Sin(jiggleTimer * jiggleFreq) * jiggleIntensity;
        }
    }

    
}
