using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIScript : MonoBehaviour
{
    [SerializeField] Transform goal;
    NavMeshAgent agent;
    Vector3 goalStartPosition;
    // Start is called before the first frame update
    void Start()
    {
        goalStartPosition = goal.position;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(goal.position != goalStartPosition)
        {
            agent.destination = goal.position;
            goalStartPosition = goal.position;
        }
    }
}
