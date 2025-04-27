using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrapple : MonoBehaviour
{
    public float shootSpeed = 30f;
    public float returnSpeed = 20f;
    public float grappleMoveSpeed = 15f;
    public float maxGrappleDistance = 20f;
    public float sphereCastRadius = 0.5f;
    public Transform player;
    public Transform grappleArm;
    public LayerMask grappleLayer;
    public float grappleResetTime = 1f; // Time to wait before resetting grapple arm

    private Vector3 shootDirection;
    private bool isShooting = false;
    private bool isReturning = false;
    private bool isPullingPlayer = false;
    private Vector3 grappleTargetPosition;
    public Animator animator;

    private CharacterController controller;
    private PlayerInputActions inputActions;
    private GrappleHand grappleHand;

    private Coroutine resetCoroutine;  // To handle resetting the grapple arm

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.Grapple.performed += OnGrappleInput;
    }

    void OnDisable()
    {
        inputActions.Gameplay.Grapple.performed -= OnGrappleInput;
        inputActions.Gameplay.Disable();
    }

    void Start()
    {
        controller = player.GetComponent<CharacterController>();
        grappleHand = GetComponent<GrappleHand>();
    }

    void Update()
    {
        if (isShooting)
        {
            MoveGrappleArm();
            DetectGrappleTarget();
        }

        if (isReturning)
        {
            ReturnGrappleArm();
        }

        if (isPullingPlayer)
        {
            PullPlayerToTarget();
        }
    }

    void OnGrappleInput(InputAction.CallbackContext context)
    {
        if (!isShooting && !isReturning && !isPullingPlayer)
        {
            ShootGrappleArm();
        }
    }

    void ShootGrappleArm()
    {
        grappleArm.gameObject.SetActive(true);
        isShooting = true;
        isReturning = false;
        isPullingPlayer = false;

        shootDirection = Camera.main.transform.forward;

        // Rotate the grapple arm to face shoot direction
        grappleArm.rotation = Quaternion.LookRotation(shootDirection);

        // Start the reset timer
        if (resetCoroutine != null)
            StopCoroutine(resetCoroutine);

        resetCoroutine = StartCoroutine(ResetGrappleAfterDelay());
    }

    void MoveGrappleArm()
    {
        grappleArm.position += shootDirection * shootSpeed * Time.deltaTime;

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
        Vector3 direction = (grappleTargetPosition - player.position);
        float distanceToMove = grappleMoveSpeed * Time.deltaTime;

        if (direction.magnitude <= distanceToMove)
        {
            controller.Move(direction);
            isPullingPlayer = false;
            isReturning = true;
        }
        else
        {
            controller.Move(direction.normalized * distanceToMove);
        }
    }

    void DetectGrappleTarget()
    {
        RaycastHit hit;
        if (Physics.SphereCast(grappleArm.position, sphereCastRadius, shootDirection, out hit, maxGrappleDistance, grappleLayer))
        {
            if (hit.collider.CompareTag("GrappleTarget"))
            {
                Debug.Log("Hit a grapple target! Pulling player after delay.");
                isShooting = false;
                grappleTargetPosition = hit.point;

                if (grappleHand != null)
                {
                    animator.SetTrigger("Grapple");
                    grappleHand.FireToTarget(grappleTargetPosition);
                }

                StartCoroutine(DelayBeforePull());
            }
        }
    }

    IEnumerator DelayBeforePull()
    {
        yield return new WaitForSeconds(0.4f);
        isPullingPlayer = true;
    }

    // Coroutine to reset the grapple arm after the specified delay
    private IEnumerator ResetGrappleAfterDelay()
    {
        yield return new WaitForSeconds(grappleResetTime);

        // Reset the grapple arm position and stop any movement
        if (grappleArm != null)
        {
            grappleArm.position = player.position;  // Reset to the player's position or other reset logic
            grappleArm.gameObject.SetActive(false);  // Optionally disable the grapple arm
        }

        isShooting = false;
        isReturning = false;
        isPullingPlayer = false;

        // Additional reset logic if needed
    }
}
