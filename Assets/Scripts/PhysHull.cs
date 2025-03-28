using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysHull : MonoBehaviour
{
    public GameObject parentHull;
    public GameObject child;
    public float mult = 1f;
    public float height = 1f;
    
    private Rigidbody parentHullRb;
    private Rigidbody childRb;
    private Vector3 offset;

    private Vector3 lastPosition;
    private Vector3 currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        parentHullRb = parentHull.GetComponent<Rigidbody>();
        childRb = child.GetComponent<Rigidbody>();

        child.transform.parent = parentHull.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //childRb.velocity = parentHullRb.velocity;
        //print(parentHullRb.velocity);


    }

    private void FixedUpdate()
    {
        //currentVelocity = (parentHullRb.transform.position - lastPosition) / Time.fixedDeltaTime;
        //lastPosition = parentHullRb.transform.position;

        //Vector3 velocityDifference = currentVelocity - childRb.velocity;
        //childRb.AddForce(new Vector3(velocityDifference.x, 0, velocityDifference.z), ForceMode.VelocityChange);
    }

    
}
