using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MechAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject point1;
    public GameObject point2;
    public GameObject point3;

    public float closeDist = 1f;

    private int count;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance <= closeDist)
        {
            count++;
            if (count >= 3) count = 0;
            
            
        }
        switch (count)
        {
            case 0:
                agent.SetDestination(point1.transform.position);
                break;
            case 1:
                agent.SetDestination(point2.transform.position);
                break;
            case 2:
                agent.SetDestination(point3.transform.position);
                break;
                    
        }
    }
}
