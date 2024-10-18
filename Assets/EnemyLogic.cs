using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    [Header("Player Detection")]
    public string playerTag = "enemy"; // Tag to identify the player
    public float detectionRadius = 10f; // Adjustable distance for the monster to detect the player
    public float followSpeed = 3.5f; // Adjustable follow speed

    [Header("Torch Settings")]
    public Light playerTorch; // Reference to player's torch light
    public Transform[] hidingSpots; // Places the monster can hide when torch is pointed at it
    public float fleeSpeed = 5f; // Speed at which the monster flees

    private NavMeshAgent agent;
    private GameObject player;
    private bool isFollowingPlayer = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(playerTag); // Find the player using tag
    }

    void Update()
    {
        // Check if the player is within detection range
        if (player != null && !isFollowingPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= detectionRadius)
            {
                isFollowingPlayer = true; // Start following the player
            }
        }

        // Follow the player if detected
        if (isFollowingPlayer)
        {
            agent.speed = followSpeed;
            agent.SetDestination(player.transform.position);

            // Check if the player's torch is hitting the monster
            if (IsTorchHittingMonster())
            {
                FleeToClosestHidingSpot();
                isFollowingPlayer = false;
            }
        }
    }

    // Use Raycast to check if the torch light is hitting the monster
    bool IsTorchHittingMonster()
    {
        if (playerTorch == null || !playerTorch.enabled)
            return false;

        RaycastHit hit;
        Vector3 directionToMonster = transform.position - playerTorch.transform.position;

        if (Physics.Raycast(playerTorch.transform.position, directionToMonster, out hit))
        {
            if (hit.transform == this.transform) // Light is hitting the monster
            {
                return true;
            }
        }

        return false;
    }

    // Flee to the closest hiding spot
    void FleeToClosestHidingSpot()
    {
        Transform closestHidingSpot = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform hidingSpot in hidingSpots)
        {
            float distance = Vector3.Distance(transform.position, hidingSpot.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHidingSpot = hidingSpot;
            }
        }

        if (closestHidingSpot != null)
        {
            agent.speed = fleeSpeed;
            agent.SetDestination(closestHidingSpot.position); // Move to hiding spot
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection radius in scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}
