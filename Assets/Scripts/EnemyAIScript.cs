using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIScript : MonoBehaviour
{
    [SerializeField] Transform goal;
    NavMeshAgent agent;
    Vector3 goalStartPosition;

    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    Vector3 lastSpawnPosition;
    Vector3 retreatPosition;

    int idleCounter;
    int chaseCounter; //not doing anything for now...

    float timer;

    bool inPursuit;
    bool retreating;
    bool investigating;

    [SerializeField] LightScript lightScript;
    [SerializeField] PlayerScript playerScript;

    [SerializeField] int teleportTimer;
    // Start is called before the first frame update
    void Start()
    {
        lastSpawnPosition = Vector3.zero;
        timer = 0;
        idleCounter = 0;
        chaseCounter = 0;
        goalStartPosition = goal.position;
        agent = GetComponent<NavMeshAgent>();
        inPursuit = false;
        retreating = false;
        investigating = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 1.0f)
        {
            idleCounter++;
            chaseCounter++;
            timer = 0;
        }

        if(idleCounter > teleportTimer && (!inPursuit || !retreating || !investigating))
        {
            MoveToRandomPosition();
            idleCounter = 0;
        }

        if(inPursuit)
        {
            if (goal.position != goalStartPosition)
            {
                agent.destination = goal.position;
                goalStartPosition = goal.position;
            }
        }

        if(lightScript.attacking)
        {
            RetreatToPoint();
        }

        if(retreating)
        {
            if(agent.remainingDistance <= 0)
            {
                retreating = false;
            }
        }

        if (playerScript.stayedTooLong && (!inPursuit || !retreating || !investigating))
        {
            agent.destination = playerScript.destination;
            investigating = true;
        }

        if(investigating)
        {
            if(agent.remainingDistance <= 0)
            {
                investigating = false;
            }
        }
    }

    void MoveToRandomPosition()
    {
        agent.enabled = false;
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
        while(lastSpawnPosition == spawnPoints[randomIndex].position)
        {
            randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
        }
        transform.position = spawnPoints[randomIndex].position;
        agent.enabled = true;
        lastSpawnPosition = spawnPoints[randomIndex].position;
    }

    void RetreatToPoint()
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
        while(lastSpawnPosition == spawnPoints[randomIndex].position || retreatPosition == spawnPoints[randomIndex].position)
        {
            randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
        }
        agent.destination = spawnPoints[randomIndex].position;
        if(inPursuit)
        {
            inPursuit = false;
        }
        retreating = true;
        retreatPosition = spawnPoints[randomIndex].position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform == goal)
        {
            agent.destination = goal.position;
            inPursuit = true;
        }
    }
}
