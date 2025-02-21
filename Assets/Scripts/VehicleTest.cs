using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleTest : MonoBehaviour
{
    Rigidbody rb;
    public bool isRb;
    public GameObject otherRb;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!isRb) rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRb && Input.GetKey(KeyCode.I))
        {
            rb.AddForce(1, 0, 0, ForceMode.Acceleration);
        }
        if (isRb)
        {
            transform.position = otherRb.transform.position + new Vector3(0, 1, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("akldirfughaolirdufghub");
        if (!isRb) 
        {
            other.transform.parent = gameObject.transform;
            
        }
        
    }
}
