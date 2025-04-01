using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;
    public Transform playerModel;
    public Image crosshair; 
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float glideGravity = -2f;
    public float minGlideHeight = 2f;
    public float mouseSensitivity = 2f;
    public float cameraPitchLimit = 80f;
    public float squashScale = 0.5f;
    public float squashSpeed = 5f;

    private Vector3 velocity;
    private bool isGrounded;
    private float cameraPitch = 0f;
    private Vector3 originalScale;
    private bool isSquashing;
    private bool isGliding;

    void Start()
    {
        originalScale = playerModel.localScale;
        crosshair.enabled = true;

        // Hide and lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Press ESC to unlock cursor
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            velocity.y = -2f;
            isGliding = false;
        }

        HandleMovement();
        HandleSquashing();
        HandleGliding();

        velocity.y += (isGliding ? glideGravity : gravity) * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -cameraPitchLimit, cameraPitchLimit);
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDirection = transform.forward * direction.z + transform.right * direction.x;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }

        else if (isGrounded)
        {
            velocity.x = 0f; // Stop residual movement
            velocity.z = 0f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void HandleSquashing()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isSquashing = true;
        }

        else
        {
            isSquashing = false;
        }

        Vector3 targetScale = isSquashing
            ? new Vector3(originalScale.x * 1.2f, originalScale.y * squashScale, originalScale.z * 1.2f)
            : originalScale;
        playerModel.localScale = Vector3.Lerp(playerModel.localScale, targetScale, Time.deltaTime * squashSpeed);
    }

    void HandleGliding()
    {
        if (!isGrounded && transform.position.y > minGlideHeight && Input.GetKey(KeyCode.Space))
        {
            isGliding = true;
        }
        else
        {
            isGliding = false;
        }
    }
}
