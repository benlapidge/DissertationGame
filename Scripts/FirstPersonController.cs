using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{

    public bool CanMove { get; private set; } = true;
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    private bool ShouldJump => Input.GetKey(jumpKey) && characterController.isGrounded;

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] bool headBobEnable = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 8.0f;
    [SerializeField] private float sprintSpeed = 16.0f;

  
    [Header("View Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;
    

    [Header("Jumping Parameters")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 30.0f;

    [Header("HeadBob")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 20f;
    [SerializeField] private float sprintBobAmount = 0.1f;

    private float defaultYPos = 1.0f;
    private float timer;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    // Start is called before the first frame update
    void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseInput();
            ApplyFinalMovements();

            if (canJump)
            {
                HandleJump();
            }

            if (headBobEnable)
            {
                HeadBobber();
            }
        } 
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        currentInput.Normalize();

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * (currentInput.x * (IsSprinting ? sprintSpeed : walkSpeed)) + (transform.TransformDirection(Vector3.right) * (currentInput.y * (IsSprinting ? sprintSpeed : walkSpeed))));
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseInput()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    void HeadBobber()
    {
        if (!characterController.isGrounded) return;

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * (IsSprinting ? sprintBobAmount : walkBobAmount), playerCamera.transform.localPosition.z);
        }

    }

    private void HandleJump()
    {
        if (ShouldJump)
            moveDirection.y = jumpForce;
    }


    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }
}
