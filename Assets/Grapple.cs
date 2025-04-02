using UnityEngine;

public class Grapple : MonoBehaviour
{
    public Transform player;                 // Player object
    public Transform grapplePoint;           // Point where the grapple connects
    public float grappleDistance = 10f;      // Maximum grapple distance
    public float pullSpeed = 10f;            // Speed at which the player is pulled
    public LayerMask grappleableLayer;       // Layer for grapple-able objects
    public Camera playerCamera;              // Reference to the camera
    public Transform crosshair;              // Reference to the crosshair (UI element)

    private LineRenderer lineRenderer;       // Line for the grapple rope
    private bool isGrappling = false;        // Whether the player is grappling

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;   // Two points: player and grapple point
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))   // Fire1 to start the grapple (e.g., mouse button)
        {
            StartGrapple();
        }

        if (isGrappling)
        {
            PullPlayer();
            UpdateGrappleLine();
        }
    }

    void StartGrapple()
    {
        // Convert the crosshair screen position to world position
        Vector3 crosshairScreenPosition = new Vector3(crosshair.position.x, crosshair.position.y, playerCamera.nearClipPlane);
        Vector3 crosshairWorldPosition = playerCamera.ScreenToWorldPoint(crosshairScreenPosition);

        // Raycast from the player camera towards the crosshair position in world space
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, (crosshairWorldPosition - playerCamera.transform.position).normalized, out hit, grappleDistance, grappleableLayer))
        {
            grapplePoint.position = hit.point;   // Set the grapple point at the hit location
            isGrappling = true;
        }
    }

    void PullPlayer()
    {
        // Move player towards the grapple point
        Vector3 direction = (grapplePoint.position - player.position).normalized;
        player.position = Vector3.MoveTowards(player.position, grapplePoint.position, pullSpeed * Time.deltaTime);

        // If the player reaches the grapple point, stop grappling
        if (Vector3.Distance(player.position, grapplePoint.position) < 1f)
        {
            isGrappling = false;
        }
    }

    void UpdateGrappleLine()
    {
        lineRenderer.SetPosition(0, player.position);   // Start point: player
        lineRenderer.SetPosition(1, grapplePoint.position);  // End point: grapple point
    }
}
