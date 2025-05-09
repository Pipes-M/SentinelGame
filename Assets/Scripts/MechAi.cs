using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MechAi : MonoBehaviour
{
    public GameObject targ;
    public GameObject forwardTarg;
    public float lookAtTurnSpeed = 5f;
    public NavMeshAgent agent;
    public GameObject point1;
    public GameObject point2;
    public GameObject point3;
    public float patrolCloseDist = 1f;
    public float followCloseDist = 5f;
    public GameObject mechPelivsParent;
    public bool alwaysLook;


    public int state;

    private GameObject player;
    private int patrolCount;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.player != null)
        {
            player = GameManager.Instance.player;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Patrol();
        //Follow();
        if (alwaysLook) LookTowards(targ);
        switch (state)
        {
            case 0:
                Hold();
                break;
            case 1:
                Follow();
                break;
            case 2:
                Patrol();
                break;
        }
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
        if (Vector3.Distance(player.transform.position, transform.position) <= followCloseDist)
        {
            agent.ResetPath();
        }
        else
        {
            agent.SetDestination(player.transform.position);
        }
    }

    void Hold()
    {
        agent.ResetPath();
    }

    public void StateSelect(int index)
    {
        state = index;
        //print(state);
    }

    void LookTowards(GameObject target)
    {
        mechPelivsParent.transform.rotation = Quaternion.Euler(0, Mathf.LerpAngle(mechPelivsParent.transform.eulerAngles.y,
            LookAt(target != null ? target : forwardTarg), lookAtTurnSpeed * Time.deltaTime), 0);
    }

    float LookAt(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.y = 0; // keep it horizontal

        if (direction.sqrMagnitude > 0.001f) // avoid division by zero
        {
            // Calculate the angle in degrees between forward and target direction


            // Apply rotation around Y axis
            //MechPelivs.transform.eulerAngles = new Vector3(-90, 90, angle);

        }
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        return angle;
    }
}
