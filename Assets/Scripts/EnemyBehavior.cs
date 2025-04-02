using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float detectionRange = 5f;

    private int currentWaypointIndex = 0;
    private Transform targetWaypoint;
    private bool isRotating = false;
    private Transform player;
    private bool isChasing = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component

        if (waypoints.Length > 0)
        {
            targetWaypoint = waypoints[currentWaypointIndex];
        }
        else
        {
            Debug.LogWarning("No waypoints assigned to SkeletonGuard.");
        }
    }

    void FixedUpdate()
    {
        // If chasing player, move toward the player
        if (isChasing && player != null)
        {
            ChasePlayer();
        }
        // If not chasing, move between waypoints
        else if (!isRotating)
        {
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        // If the skeleton is close to the current waypoint, move to the next
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f)
        {
            StartCoroutine(RotateToNextWaypoint());
        }
        else
        {
            Vector3 direction = (targetWaypoint.position - transform.position).normalized;
            direction.y = 0; // Keep the movement on the ground

            Move(direction);
            RotateTowards(direction);
        }
    }

    void RotateTowards(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
        }
    }

    IEnumerator RotateToNextWaypoint()
    {
        isRotating = true;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
            yield return null;
        }

        rb.MoveRotation(targetRotation);
        targetWaypoint = waypoints[currentWaypointIndex];
        isRotating = false;
    }

    // Trigger area for chasing the player (no damage yet)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isChasing = true;
        }
    }

    // Stop chasing when player leaves trigger area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
            player = null;
            // If the player leaves, return to waypoint movement
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }

    // Detect when the skeleton physically collides with the player to deal damage
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(100); // Call TakeDamage() to apply damage and potentially kill
            }
        }
    }

    void ChasePlayer()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position;
            targetPosition.y = transform.position.y; // Keep skeleton on the ground

            Vector3 direction = (targetPosition - transform.position).normalized;
            Move(direction);
            RotateTowards(direction);
        }
    }

    void Move(Vector3 direction)
    {
        // Use MovePosition for smooth physics movement
        Vector3 targetPosition = transform.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition); // Moves skeleton smoothly
    }
}