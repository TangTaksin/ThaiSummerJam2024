using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Check if there are patrol points
        if (patrolPoints.Length > 1 && !waiting)
        {
            // Calculate direction to move
            Vector3 direction = (patrolPoints[currentPatrolIndex].position - transform.position).normalized;

            // Move towards the current patrol point
            transform.Translate(direction * moveSpeed * Time.deltaTime);

            // Check if the enemy reached the current patrol point
            if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.1f)
            {
                // Change direction if reached the current point
                if (movingForward)
                {
                    currentPatrolIndex++;
                    if (currentPatrolIndex >= patrolPoints.Length)
                    {
                        currentPatrolIndex = patrolPoints.Length - 2;
                        movingForward = false;
                    }
                }
                else
                {
                    currentPatrolIndex--;
                    if (currentPatrolIndex < 0)
                    {
                        currentPatrolIndex = 1;
                        movingForward = true;
                    }
                }

                // Start waiting if shouldWaitAtPatrolPoints is true
                if (shouldWaitAtPatrolPoints)
                {
                    waiting = true;
                    waitTimer = waitTime;
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
            }
        }
    }

    void Chase()
    {
        // Calculate direction to move towards the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Move towards the player
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
