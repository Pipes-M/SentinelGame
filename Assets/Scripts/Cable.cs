using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    public Vector3 dest;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(dest, transform.position);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, dist / 2);
        transform.LookAt(dest);
    }
}
