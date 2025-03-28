using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipPda : MonoBehaviour
{
    public float lerpSpeed = 1f;
    public GameObject PdaEmpty;
    public GameObject PdaActual;
    public GameObject camPoint;
    public GameObject hidePoint;
    private float lerp;
    private float easedLerp;
    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        PdaEmpty.transform.position = hidePoint.transform.position;
        PdaEmpty.transform.rotation = hidePoint.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && lerp < 1)
        {
            lerp += (Time.deltaTime * lerpSpeed);
            easedLerp = Mathf.Sin(lerp * Mathf.PI * 0.5f);
        }
        if (!active && lerp > 0)
        {
            lerp -= Time.deltaTime * lerpSpeed;
            easedLerp = Mathf.Sin(lerp * Mathf.PI * 0.5f);
        }


        PdaEmpty.transform.position = Vector3.Lerp(hidePoint.transform.position, camPoint.transform.position, easedLerp);
        PdaEmpty.transform.rotation = Quaternion.Lerp(hidePoint.transform.rotation, camPoint.transform.rotation, easedLerp);


        if (lerp <= 0)
        {
            PdaActual.SetActive(false);
        }
        else
        {
            PdaActual.SetActive(true);
            //print("alieudrfyhsgaol");
        }
        
    }

    public void ActivatePda(bool shouldActive)
    {
        if (shouldActive)
        {
            active = true;
        }
        else
        {
            active = false;
        }
    }
}
