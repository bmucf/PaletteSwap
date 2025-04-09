using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float detectionRange = 5f;

    private int currentWaypointIndex = 0;
    private Transform targetWaypoint;
    private Transform player;
    private bool isChasing = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (waypoints.Length > 0)
        {
            targetWaypoint = waypoints[currentWaypointIndex];
        }
        else
        {
            Debug.LogWarning("No waypoints assigned.");
        }
    }

    void FixedUpdate()
    {
        if (isChasing && player != null)
        {
            ChasePlayer();
        }
        else
        {
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 1.0f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            targetWaypoint = waypoints[currentWaypointIndex];
        }

        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        direction.y = 0;

        Move(direction);
        RotateTowards(direction);
    }

    void RotateTowards(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isChasing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
            player = null;
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }

    void ChasePlayer()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position;
            targetPosition.y = transform.position.y;

            Vector3 direction = (targetPosition - transform.position).normalized;
            Move(direction);
            RotateTowards(direction);
        }
    }

    void Move(Vector3 direction)
    {
        Vector3 targetPosition = transform.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }
}
