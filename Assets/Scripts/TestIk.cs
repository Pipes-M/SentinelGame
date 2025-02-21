using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIk : MonoBehaviour
{
    public float distNewStep = 10f;
    public float legHeightAdjust = 1f;
    public float rayLength = 100f;
    public GameObject ikTip;
    private Vector3 firstStep;
    private Vector3 secondStep;
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int layerToIgnore = LayerMask.NameToLayer("Test");
        int layerMask = ~(1 << layerToIgnore);

        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength, layerMask))
        {
            ikTip.transform.position = hit.point + new Vector3(0, legHeightAdjust, 0);
        }
        Debug.DrawRay(ray.origin + transform.right * -2f, ray.direction * rayLength, Color.red);
    }
}
