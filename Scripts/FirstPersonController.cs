using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Functional Options")] [SerializeField]
    private bool canSprint = true;

    [SerializeField] private bool canJump = true;
    [SerializeField] private bool headBobEnable = true;
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool footSteps = true;

    [Header("Controls")] [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Movement Parameters")] [SerializeField]
    private float walkSpeed = 8.0f;

    [SerializeField] private float sprintSpeed = 16.0f;


    [Header("View Parameters")] [SerializeField] [Range(1, 10)]
    private float lookSpeedX = 2.0f;

    [SerializeField] [Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField] [Range(1, 180)] private float upperLookLimit = 80.0f;
    [SerializeField] [Range(1, 180)] private float lowerLookLimit = 80.0f;


    [Header("Jumping Parameters")] [SerializeField]
    private float jumpForce = 8.0f;

    [SerializeField] private float gravity = 30.0f;

    [Header("HeadBob")] [SerializeField] private float walkBobSpeed = 14f;

    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 20f;
    [SerializeField] private float sprintBobAmount = 0.1f;

    [Header("Interaction")] [SerializeField]
    private Vector3 interactionRayPoint;

    [SerializeField] private float interactionDistance;
    [SerializeField] private LayerMask interactionLayer;

    [Header("Footsteps")] 
    [SerializeField] private float baseStepSpeed = 0.5f;
    [SerializeField] private float sprintMultiplier = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] stepClips = default;
    private float footstepTimer = 0;
    private float GetCurrentOffset => IsSprinting ? baseStepSpeed * sprintMultiplier : baseStepSpeed;
    


    private readonly float defaultYPos = 1.0f;
    private CharacterController characterController;
    private Vector2 currentInput;
    private Interactable currentInteractable;

    private Vector3 moveDirection;


    private Camera playerCamera;

    private float rotationX;
    private float timer;

    /*
     * Used this tutorial series to build this system
     * https://www.youtube.com/watch?v=AQc-NM2Up3M&list=PLfhbBaEcybmgidDH3RX_qzFM0mIxWJa21&index=8
     * 
     */

    public bool CanMove { get; } = true;
    private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    private bool ShouldJump => Input.GetKey(jumpKey) && characterController.isGrounded;

    // Start is called before the first frame update
    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseInput();
            ApplyFinalMovements();

            if (canJump) HandleJump();

            if (headBobEnable) HeadBobber();

            if (footSteps) HandleFootSteps();

            if (canInteract)
            {
                HandleInteractionCheck();
                HandleInteractionInput();
            }
        }
    }

    private void HandleInteractionCheck()
    {
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out var hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer == 9 && (currentInteractable == null ||
                                                       hit.collider.gameObject.GetInstanceID() !=
                                                       currentInteractable.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);

                if (currentInteractable) currentInteractable.OnFocus();
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnFocusLost();
            currentInteractable = null;
        }
    }


    private void HandleInteractionInput()
    {
        if (Input.GetKey(interactKey) && currentInteractable != null && Physics.Raycast(
                playerCamera.ViewportPointToRay(interactionRayPoint), out var hit, interactionDistance,
                interactionLayer)) currentInteractable.OnInteract();
    }

    private void HandleMovementInput()
    {
        currentInput = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        currentInput.Normalize();

        var moveDirectionY = moveDirection.y;
        moveDirection =
            transform.TransformDirection(Vector3.forward) * (currentInput.x * (IsSprinting ? sprintSpeed : walkSpeed)) +
            transform.TransformDirection(Vector3.right) * (currentInput.y * (IsSprinting ? sprintSpeed : walkSpeed));
        moveDirection.y = moveDirectionY;
    }

    private void HandleMouseInput()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void HeadBobber()
    {
        if (!characterController.isGrounded) return;

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (IsSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (IsSprinting ? sprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    private void HandleJump()
    {
        if (ShouldJump)
            moveDirection.y = jumpForce;
    }
    
    private void HandleFootSteps(){
        if (!characterController.isGrounded) return;
        if (currentInput == Vector2.zero) return;
        
        footstepTimer -= Time.deltaTime;
        
        if (footstepTimer <= 0)
        {
            footstepAudioSource.PlayOneShot(stepClips[Random.Range(0, stepClips.Length -1)]);
            footstepTimer = GetCurrentOffset;
        }

        
    }


    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }
}