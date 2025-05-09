using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamAttach : MonoBehaviour
{
    public float centerDuration = 0.5f;
    private GameObject origParent;

    // Start is called before the first frame update
    void Start()
    {
        origParent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveCam(bool returnToPlayer, GameObject objParent)
    {
        if (returnToPlayer)
        {
            //StartCoroutine(CamMove(origParent));
            transform.parent = origParent.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        else
        {
            StartCoroutine(CamMove(objParent));
        }
    }

    IEnumerator CamMove(GameObject parentObj)
    {
        transform.parent = parentObj.transform;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;
        Quaternion endRot = Quaternion.identity;
        float time = 0f;
        while (time < centerDuration)
        {
            float lerp = time / centerDuration;
            transform.localPosition = Vector3.Lerp(startPos, Vector3.zero, lerp);
            transform.rotation = Quaternion.Lerp(startRot, endRot, lerp);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = Vector3.zero;
        transform.rotation = endRot;
    }
}
