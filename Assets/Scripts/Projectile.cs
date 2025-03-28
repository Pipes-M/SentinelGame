using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    public float force = 10f;
    public GameObject owner;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * force);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        //print(collision.gameObject);
        if (collision.gameObject.GetComponent<Health>() != null) // && collision.gameObject != owner
        {
            collision.gameObject.GetComponent<Health>().Damage(damage);
        }
        else if (GetRootParent(collision.transform).gameObject.GetComponent<Health>() != null) // && collision.gameObject != owner
        {
            GetRootParent(collision.transform).gameObject.GetComponent<Health>().Damage(damage);
        }
        Destroy(gameObject);
    }

    Transform GetRootParent(Transform child)
    {
        while (child.parent != null)
        {
            child = child.parent;
        }
        return child;
    }
}
