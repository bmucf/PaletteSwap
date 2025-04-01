using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 10f;
    public float visionAngle = 45f;
    public float loseSightTime = 2f;
    public Transform[] patrolPoints;
    public Transform player;
    public LayerMask playerLayer;  // New LayerMask to only detect the player
    private int currentPatrolIndex;
    private bool chasingPlayer = false;
    private float timeSinceLastSeen = 0f;

    void Start()
    {
        currentPatrolIndex = Random.Range(0, patrolPoints.Length);
        StartCoroutine(Patrol());
    }

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        bool playerInSight = false;

        // Debug Vision Cone (Field of View)
        Debug.DrawRay(transform.position, transform.forward * detectionRange, Color.green); // Green: Forward Direction
        Debug.DrawRay(transform.position, directionToPlayer * detectionRange, Color.red); // Red: Direction to Player

        if (Vector3.Distance(transform.position, player.position) <= detectionRange && angleToPlayer <= visionAngle)
        {
            RaycastHit hit;
            Vector3 rayOrigin = transform.position + Vector3.up * 1.5f; // Raise raycast origin to enemy's "eyes"

            // Use LayerMask to ensure we only hit the player
            if (Physics.Raycast(rayOrigin, directionToPlayer, out hit, detectionRange, playerLayer))
            {
                Debug.Log("Raycast hit: " + hit.collider.name); // Debug raycast hit
                if (hit.collider.CompareTag("Player"))
                {
                    playerInSight = true;
                    timeSinceLastSeen = 0f;
                    if (!chasingPlayer)
                    {
                        chasingPlayer = true;
                        StopCoroutine(Patrol());
                        StartCoroutine(ChasePlayer());
                    }
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }

        if (!playerInSight && chasingPlayer)
        {
            timeSinceLastSeen += Time.deltaTime;
            if (timeSinceLastSeen >= loseSightTime)
            {
                chasingPlayer = false;
                StopCoroutine(ChasePlayer());
                StartCoroutine(Patrol());
            }
        }
    }

    IEnumerator Patrol()
    {
        while (!chasingPlayer)
        {
            Transform targetPoint = patrolPoints[currentPatrolIndex];
            while (Vector3.Distance(transform.position, targetPoint.position) > 0.2f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);
                transform.LookAt(targetPoint);
                yield return null;
            }

            currentPatrolIndex = Random.Range(0, patrolPoints.Length);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    IEnumerator ChasePlayer()
    {
        while (chasingPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            transform.LookAt(player);
            yield return null;
        }
    }
}
