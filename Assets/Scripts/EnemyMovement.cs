using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform player;
    public float moveSpeed = 2f;
    public float chaseRange = 5f;
    public float waitTime = 1.0f; // Time to wait at each patrol point
    public bool shouldWaitAtPatrolPoints = true; // Whether the enemy should wait at patrol points or not

    private int currentPatrolIndex = 0;
    private bool movingForward = true;
    private bool waiting = false;
    private float waitTimer = 0f;

    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {

        if (player != null && Vector3.Distance(transform.position, player.position) <= chaseRange)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length > 1 && !waiting)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);

            if (agent.remainingDistance < 0.1f)
            {
                if (shouldWaitAtPatrolPoints)
                {
                    waiting = true;
                    movingForward = false;
                    waitTimer = waitTime;
                }
                else
                {
                    // Change direction if reached the current point
                    if (movingForward && !waiting)
                    {
                        currentPatrolIndex++;
                        if (currentPatrolIndex >= patrolPoints.Length)
                        {
                            currentPatrolIndex = 0; // Reset to the beginning
                        }
                    }
                    else
                    {
                        currentPatrolIndex--;
                        if (currentPatrolIndex < 0)
                        {
                            currentPatrolIndex = patrolPoints.Length - 1; // Set to the last point
                        }
                    }
                }
            }
        }
        else if (waiting)
        {
            // Reduce the wait timer
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                waiting = false;
                movingForward = true;
            }
        }
    }

    void Chase()
    {
        agent.SetDestination(player.position);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
