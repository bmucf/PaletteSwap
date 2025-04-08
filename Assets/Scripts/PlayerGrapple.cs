using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    public float shootSpeed = 30f; // Speed of the grapple arm
    public float returnSpeed = 20f; // Speed of returning the grapple arm
    public float grappleMoveSpeed = 15f; // Speed at which player moves to the target
    public float maxGrappleDistance = 20f; // Max range of grapple
    public float sphereCastRadius = 0.5f; // Radius of the sphere cast for detecting targets
    public Transform player; // The player
    public Transform grappleArm; // The grapple arm (small cube)
    public LayerMask grappleLayer; // Layer for valid grapple targets

    private Vector3 shootDirection; // Direction the grapple arm moves
    private bool isShooting = false;
    private bool isReturning = false;
    private bool isPullingPlayer = false;
    private Vector3 grappleTargetPosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isShooting && !isReturning && !isPullingPlayer)
        {
            ShootGrappleArm();
        }

        if (isShooting)
        {
            MoveGrappleArm();
        }

        if (isReturning)
        {
            ReturnGrappleArm();
        }

        if (isPullingPlayer)
        {
            PullPlayerToTarget();
        }

        // Check for a grapple target using SphereCast
        if (isShooting)
        {
            DetectGrappleTarget();
        }
    }

    void ShootGrappleArm()
    {
        grappleArm.gameObject.SetActive(true);
        isShooting = true;
        isReturning = false;
        isPullingPlayer = false;

        // Set the direction based on where the player is looking
        shootDirection = Camera.main.transform.forward;
    }

    void MoveGrappleArm()
    {
        grappleArm.position += shootDirection * shootSpeed * Time.deltaTime;

        // If the arm goes too far without hitting anything, return
        if (Vector3.Distance(grappleArm.position, player.position) > maxGrappleDistance)
        {
            isShooting = false;
            isReturning = true;
        }
    }

    void ReturnGrappleArm()
    {
        grappleArm.position = Vector3.MoveTowards(grappleArm.position, player.position, returnSpeed * Time.deltaTime);

        if (Vector3.Distance(grappleArm.position, player.position) < 0.5f)
        {
            isReturning = false;
        }
    }

    void PullPlayerToTarget()
    {
        player.position = Vector3.MoveTowards(player.position, grappleTargetPosition, grappleMoveSpeed * Time.deltaTime);

        if (Vector3.Distance(player.position, grappleTargetPosition) < 0.5f)
        {
            isPullingPlayer = false;
            isReturning = true;
        }
    }

    // Detect grapple target using SphereCast
    void DetectGrappleTarget()
    {
        RaycastHit hit;
        if (Physics.SphereCast(grappleArm.position, sphereCastRadius, shootDirection, out hit, maxGrappleDistance, grappleLayer))
        {
            if (hit.collider.CompareTag("GrappleTarget"))
            {
                Debug.Log("Hit a grapple target! Pulling player.");
                isShooting = false;
                isPullingPlayer = true;
                grappleTargetPosition = hit.point; // Set target to hit point
            }
        }
    }
}
