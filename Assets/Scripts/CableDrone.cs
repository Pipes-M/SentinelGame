using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;

public class CableDrone : MonoBehaviour
{
    public GameObject droneBody;
    public GameObject gunObj;
    public float visionRange = 20f;
    public float visionAngle = 60f;
    public NavMeshAgent agent;
    public GameObject point1;
    public GameObject point2;
    public GameObject point3;
    public float patrolCloseDist = 1f;
    public float followCloseDist = 5f;
    public GameObject eyeBase;
    public GameObject eyeBlock;
    public GameObject metalObj;
    public GameObject plasticObj;

    private int patrolCount;
    private GameObject playerObj;
    private int layerToIgnore;
    private int layerMask;
    private bool playerInLos;
    private bool playerInSight;
    private Gun gunScript;

    private float nextTime = 0f;
    private float interval = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameManager.Instance.player;
        layerToIgnore = LayerMask.NameToLayer("Drone");
        layerMask = ~(1 << layerToIgnore);
        gunScript = gunObj.GetComponent<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerObj == null)
        {
            playerObj = GameManager.Instance.player;
        }

        Vector3 directionToPlayer = (playerObj.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, 10000))
        {
            if (hit.collider.gameObject == playerObj)
            {
                playerInLos = true;
            }
            else
            {
                playerInLos = false;
            }
        }

        if (playerInSight)
        {
            Follow();
            if (Time.time >= nextTime)
            {
                nextTime = Time.time + interval;
                Shoot();
            }
            
        }
        else
        {
            Patrol();
        }
        //EyeLook();
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

    void Follow()
    {
        if (Vector3.Distance(playerObj.transform.position, transform.position) <= followCloseDist)
        {
            agent.ResetPath();
        }
        else
        {
            agent.SetDestination(playerObj.transform.position);
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (playerObj.transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, playerObj.transform.position);

        if (distanceToPlayer > visionRange)
            return false;

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > visionAngle)
            return false;

        if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer))
        {
            return true;
        }

        return false;
    }

    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerInLos)
        {
            playerInSight = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInSight = false;
        }
    }

    void Shoot()
    {
        
        gunObj.transform.LookAt(playerObj.transform);
        gunScript.Fire();
        
    }

    public void Death()
    {
        Instantiate(metalObj, transform.position, Quaternion.LookRotation(transform.forward));
        Instantiate(metalObj, transform.position, Quaternion.LookRotation(transform.forward));
        Instantiate(plasticObj, transform.position, Quaternion.LookRotation(transform.forward));
        Destroy(gameObject);
    }

    void EyeLook()
    {
        if (playerInSight)
        {
            eyeBase.transform.LookAt(new Vector3(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z), Vector3.up);
            eyeBlock.transform.LookAt(new Vector3(transform.position.x, playerObj.transform.position.y, transform.position.z), Vector3.up);
        }
        else
        {
            eyeBase.transform.localRotation = Quaternion.Euler(0, 0, 0);
            eyeBlock.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
