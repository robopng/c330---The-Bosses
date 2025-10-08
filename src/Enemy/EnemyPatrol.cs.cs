using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;   // Route points (in a sequential loop)
    public float speed = 2f;           // Patrol speed
    public float stopDistance = 2f;    // Stop at a certain distance from the player
    public Transform player;           // player target

    private int currentPoint = 0;

    void Update()
    {
        if (player == null || patrolPoints.Length == 0) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= stopDistance)
        {
            // Stop moving when approaching the player
            return;
        }

        Transform target = patrolPoints[currentPoint];
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        float distToTarget = Vector3.Distance(transform.position, target.position);
        if (distToTarget < 0.1f)
        {
            // Reach the current point 
            // -> move on to the next one
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
    }
}