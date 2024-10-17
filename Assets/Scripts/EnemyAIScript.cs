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
    float timer;

    bool inPursuit;
    bool retreating;
    bool investigating;

    [SerializeField] LightScript lightScript;
    [SerializeField] PlayerScript playerScript;

    [SerializeField] int teleportTimer;

    void Start()
    {
        lastSpawnPosition = Vector3.zero;
        timer = 0;
        idleCounter = 0;
        goalStartPosition = goal.position;
        agent = GetComponent<NavMeshAgent>();
        inPursuit = false;
        retreating = false;
        investigating = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1.0f)
        {
            idleCounter++;
            timer = 0;
        }

        if (idleCounter > teleportTimer && !inPursuit && !retreating && !investigating)
        {
            MoveToRandomPosition();
            idleCounter = 0;
        }

        if (inPursuit)
        {
            if (goal.position != goalStartPosition)
            {
                agent.destination = goal.position;
                goalStartPosition = goal.position;
            }
        }

        if (lightScript.attacking)
        {
            if (!retreating)
            {
                RetreatToPoint();
            }
        }

        if (retreating)
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                retreating = false;
            }
        }

        if (playerScript.stayedTooLong && !inPursuit && !retreating && !investigating)
        {
            agent.destination = playerScript.destination;
            investigating = true;
        }

        if (investigating)
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                investigating = false; 
            }
        }
    }

    void MoveToRandomPosition()
    {
        agent.enabled = false;
        int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
        while (lastSpawnPosition == spawnPoints[randomIndex].position)
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
        while (lastSpawnPosition == spawnPoints[randomIndex].position || retreatPosition == spawnPoints[randomIndex].position)
        {
            randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
        }

        retreatPosition = spawnPoints[randomIndex].position;
        agent.SetDestination(retreatPosition);

        retreating = true;
        inPursuit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.name == "PlayerController")
        {
            agent.destination = goal.position;
            inPursuit = true;
        }
    }
}
