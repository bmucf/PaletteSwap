using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform cameraTransform;
    public Transform cameraPivot;
    public Transform playerModel;
    public Image crosshair;
    public Animator animator;
    public bool canMove = true;
    private bool wasSquashing;


    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip squashSound;
    public AudioClip jumpSound;
    public AudioClip glideSound;


    [Header("Camera Settings")]
    public Vector3 cameraOffset = new Vector3(0.5f, 0f, -4f);
    public float mouseSensitivity = 0.1f;
    public float controllerSensitivity = 2.5f;
    public float cameraPitchLimit = 80f;
    public float lookSmoothTime = 0.05f;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float glideGravity = -2f;
    public float minGlideHeight = 2f;

    [Header("Squash Settings")]
    public float squashScale = 0.5f;
    public float squashSpeed = 5f;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Vector2 lookDelta;
    private Vector2 smoothLook;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isGliding;
    private bool isSquashing;
    private bool jumpPressed;

    private float cameraPitch = 0f;
    private Vector3 originalScale;

    void Awake()
    {
        inputActions = new PlayerInputActions();

        inputActions.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Gameplay.Move.canceled += _ => moveInput = Vector2.zero;

        inputActions.Gameplay.Look.performed += ctx => lookDelta = ctx.ReadValue<Vector2>();
        inputActions.Gameplay.Look.canceled += _ => lookDelta = Vector2.zero;

        inputActions.Gameplay.Jump.performed += _ => jumpPressed = true;
    }

    void OnEnable() => inputActions.Gameplay.Enable();
    void OnDisable() => inputActions.Gameplay.Disable();

    void Start()
    {
        if (playerModel != null)
            animator = playerModel.GetComponent<Animator>();
        else
            Debug.LogError("Player Model is not assigned!");

        originalScale = playerModel.localScale;
        crosshair.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraPitch = cameraPivot.localEulerAngles.x;
        lookDelta = Vector2.zero;
    }


    void Update()
    {
        isGrounded = controller.isGrounded;

        HandleMovement();
        HandleLook();
        HandleSquashing();
        HandleGliding();
        UpdateAnimationStates();

        // Apply vertical velocity
        velocity.y += (isGliding ? glideGravity : gravity) * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Reset jump flag each frame
        jumpPressed = false;
    }
    void UpdateAnimationStates()
    {
        float horizontalSpeed = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;
        bool isMoving = horizontalSpeed > 0.1f;

        animator.SetBool("isRunning", isGrounded && isMoving);
        animator.SetBool("isGliding", isGliding);
    }



    void LateUpdate()
    {
        if (cameraPivot && playerModel)
        {
            cameraPivot.position = playerModel.position + Vector3.up * 1.5f;
            cameraTransform.localPosition = cameraOffset;
        }
    }

    void HandleMovement()
    {
        if (!canMove) return;

        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
        float speed = direction.magnitude;

       // Debug.Log($"Move Input: {moveInput}, Speed: {speed}");

        if (speed >= 0.1f)
        {
            animator.SetBool("isRunning", true);
            animator.SetFloat("Speed", speed);
            Vector3 moveDirection = transform.forward * direction.z + transform.right * direction.x;
            controller.Move(moveDirection.normalized * this.speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetFloat("Speed", 0f);
        }

        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");

            GetComponent<Animator>().SetBool("isGrounded", true);


            if (jumpSound != null)
                audioSource.PlayOneShot(jumpSound);
        }
    }


    void HandleLook()
    {
        Vector2 inputLook = Mouse.current != null
            ? Mouse.current.delta.ReadValue() * mouseSensitivity
            : Gamepad.current?.rightStick.ReadValue() * controllerSensitivity * Time.deltaTime * 100f ?? Vector2.zero;

        smoothLook = Vector2.Lerp(smoothLook, inputLook, 1 - Mathf.Exp(-Time.deltaTime / lookSmoothTime));

        transform.Rotate(Vector3.up * smoothLook.x);

        cameraPitch -= smoothLook.y;
        cameraPitch = Mathf.Clamp(cameraPitch, -cameraPitchLimit, cameraPitchLimit);
        cameraPivot.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void HandleSquashing()
    {
        isSquashing = Keyboard.current.leftCtrlKey.isPressed;

        Vector3 targetScale = isSquashing
            ? new Vector3(originalScale.x * 1.2f, originalScale.y * squashScale, originalScale.z * 1.2f)
            : originalScale;

        playerModel.localScale = Vector3.Lerp(playerModel.localScale, targetScale, Time.deltaTime * squashSpeed);

        if (isSquashing && !wasSquashing && squashSound != null)
        {
            audioSource.PlayOneShot(squashSound);
        }

        wasSquashing = isSquashing;
    }


    void HandleGliding()
    {
        bool spaceHeld = Keyboard.current.spaceKey.isPressed || Gamepad.current?.buttonSouth.isPressed == true;

        // Start gliding only if airborne, above the minimum glide height, and space is held
        if (!isGrounded && transform.position.y > minGlideHeight && spaceHeld)
        {
            isGliding = true;
            animator.SetBool("IsGliding", true);  // Set gliding state in Animator
            audioSource.PlayOneShot(glideSound);
        }
        else
        {
            isGliding = false;
            animator.SetBool("IsGliding", false);  // Stop gliding state in Animator when grounded or too low
            GetComponent<Animator>().SetBool("isGrounded", true);
        }
    }
}
