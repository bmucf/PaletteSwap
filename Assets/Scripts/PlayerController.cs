using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public Transform cameraTransform;
    public Transform cameraPivot; // New: for vertical pitch rotation
    public Transform playerModel;
    public Image crosshair;
    public Vector3 cameraOffset = new Vector3(0.5f, 0f, -4f);


    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float glideGravity = -2f;
    public float minGlideHeight = 2f;
    public float mouseSensitivity = 0.1f;
    public float controllerSensitivity = 2.5f;
    public float cameraPitchLimit = 80f;
    public float squashScale = 0.5f;
    public float squashSpeed = 5f;
    public float lookSmoothTime = 0.05f;

    private Vector3 velocity;
    private bool isGrounded;
    private float cameraPitch = 0f;
    private Vector3 originalScale;
    private bool isSquashing;
    private bool isGliding;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Vector2 lookDelta;
    private Vector2 smoothLook;
    private bool jumpPressed = false;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Gameplay.Look.performed += ctx => lookDelta = ctx.ReadValue<Vector2>();
        inputActions.Gameplay.Look.canceled += ctx => lookDelta = Vector2.zero;

        inputActions.Gameplay.Jump.performed += ctx => jumpPressed = true;
    }

    void OnEnable() => inputActions.Gameplay.Enable();
    void OnDisable() => inputActions.Gameplay.Disable();

    void Start()
    {
        originalScale = playerModel.localScale;
        crosshair.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            velocity.y = -2f;
            isGliding = false;
        }

        HandleMovement();
        HandleLook();
        HandleSquashing();
        HandleGliding();

        velocity.y += (isGliding ? glideGravity : gravity) * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        jumpPressed = false; // Reset after using
    }

    void LateUpdate()
    {
        if (cameraPivot != null && playerModel != null)
        {
            cameraPivot.position = playerModel.position + Vector3.up * 1.5f; // Adjust for shoulder height
            cameraTransform.localPosition = cameraOffset; // Fixed shoulder offset
        }
    }


void HandleMovement()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDirection = transform.forward * direction.z + transform.right * direction.x;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
        else if (isGrounded)
        {
            velocity.x = 0f;
            velocity.z = 0f;
        }

        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void HandleLook()
    {
        Vector2 inputLook = Vector2.zero;

        if (Mouse.current != null && Mouse.current.delta.ReadValue() != Vector2.zero)
        {
            inputLook = Mouse.current.delta.ReadValue() * mouseSensitivity;
        }
        else if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.rightStick.ReadValue();
            inputLook = stick * controllerSensitivity * Time.deltaTime * 100f;
        }

        smoothLook = Vector2.Lerp(smoothLook, inputLook, 1 - Mathf.Exp(-Time.deltaTime / lookSmoothTime));

        // Rotate the player horizontally (Y-axis)
        transform.Rotate(Vector3.up * smoothLook.x);

        // Rotate the cameraPivot vertically (pitch)
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
    }

    void HandleGliding()
    {
        bool spaceHeld = Keyboard.current.spaceKey.isPressed || Gamepad.current?.buttonSouth.isPressed == true;
        if (!isGrounded && transform.position.y > minGlideHeight && spaceHeld)
        {
            isGliding = true;
        }
        else
        {
            isGliding = false;
        }
    }
}
