using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerGrapple : MonoBehaviour
{
    public float grappleRange = 20f; // Range to grapple target
    public float grappleForce = 10f; // Force applied to player when grappling
    public float shootSpeed = 30f; // Speed at which the cube will shoot
    public Transform grappleArm; // Reference to the grapple arm (the small cube)
    public KeyCode grappleKey = KeyCode.E; // Key to activate the grapple

    private Transform currentGrappleTarget; // The current target to grapple to
    private bool isGrappling = false; // Is the player currently grappling?
    private bool isShooting = false; // Is the grapple arm shooting?
    private Vector3 grappleTargetPosition; // The target position where the grapple arm should shoot

    void Update()
    {
        // Check if the player presses the left mouse button (Button 0)
        if (Input.GetMouseButtonDown(0) && !isShooting && !isGrappling)
        {
            Debug.Log("Left mouse button clicked!");
            ShootGrappleArm(); // Call the method to shoot the grapple arm
        }

        // If the grapple arm is shooting, move it towards the target
        if (isShooting)
        {
            ShootArmTowardsTarget();
        }

        // If grappling, move the player towards the target
        if (isGrappling)
        {
            GrappleToTarget();
        }
    }

    private void ShootGrappleArm()
    {
        grappleArm.gameObject.SetActive(true);
        // Check for a valid target within range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, grappleRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("GrappleTarget"))
            {
                currentGrappleTarget = hitCollider.transform;
                grappleTargetPosition = currentGrappleTarget.position;
                isShooting = true;
                grappleArm.gameObject.SetActive(true); // Show the grapple arm
                break;
            }
        }
    }

    private void ShootArmTowardsTarget()
    {
        if (grappleArm == null || currentGrappleTarget == null) return;

        // Calculate direction towards the target
        Vector3 direction = (grappleTargetPosition - grappleArm.position).normalized;

        // Debug log to check if the direction is being calculated correctly
        Debug.Log($"Moving towards target: {grappleTargetPosition}, Direction: {direction}");

        // Move the grapple arm (cube) towards the target position
        grappleArm.position = Vector3.MoveTowards(grappleArm.position, grappleTargetPosition, shootSpeed * Time.deltaTime);

        // Debug log to check if the position is updating
        Debug.Log($"Grapple Arm Position: {grappleArm.position}");

        // Once the grapple arm reaches the target, start the grappling process
        if (Vector3.Distance(grappleArm.position, grappleTargetPosition) < 0.5f)
        {
            Debug.Log("Grapple arm has reached the target!");
            isShooting = false;
            isGrappling = true;
            grappleArm.gameObject.SetActive(false); // Hide the grapple arm
        }
    }

    private void GrappleToTarget()
    {
        if (currentGrappleTarget == null) return;

        // Calculate direction to the grapple target
        Vector3 directionToTarget = (currentGrappleTarget.position - transform.position).normalized;

        // Change the player's velocity to move towards the target
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = directionToTarget * grappleForce;

        // Optionally, you could rotate the player to face the target
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // Optionally, add a check to stop the grapple once the player reaches the target
        if (Vector3.Distance(transform.position, currentGrappleTarget.position) < 1f)
        {
            isGrappling = false;
            currentGrappleTarget = null;
        }
    }
}
