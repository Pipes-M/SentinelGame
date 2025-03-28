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
    public float patrolCloseDist = 1f;
    public float followCloseDist = 5f;
    
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
}
