using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CableDrone : MonoBehaviour
{
    public float cableChangeDelay = 2f;
    public GameObject droneBody;
    public List<GameObject> cables = new List<GameObject>();
    private int layerToIgnore;
    private int layerMask;

    public NavMeshAgent agent;
    public GameObject point1;
    public GameObject point2;
    public GameObject point3;
    public float patrolCloseDist = 1f;

    private int patrolCount;


    // Start is called before the first frame update
    void Start()
    {
        layerToIgnore = LayerMask.NameToLayer("Drone");
        layerMask = ~(1 << layerToIgnore);
        StartCoroutine(CableDetection());
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    IEnumerator CableDetection()
    {
        foreach (GameObject obj in cables)
        {
            print("aergar");
            Ray ray = new Ray(droneBody.transform.position, (obj.transform.position - droneBody.transform.position).normalized);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000, layerMask))
            {
                obj.GetComponent<Cable>().dest = hit.point;
            }
        }
        yield return new WaitForSeconds(cableChangeDelay);
        StartCoroutine(CableDetection());
    }

    void Patrol()
    {
        if (agent.remainingDistance <= patrolCloseDist)
        {
            patrolCount++;
            if (patrolCount >= 3) patrolCount = 0;


        }
        switch (patrolCount)
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
