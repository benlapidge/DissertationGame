using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    /*
     * tutorial used for this section
     * https://www.youtube.com/watch?v=PmIPqGqp8UY&list=PLeJwXmSx9osBEBX2ugImGd0uOilzb2cwq&index=6
     * 
     */


    //settings for player movement & camera
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float jumpSpeed = 10.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    Vector3 velocity;
    float cameraPitch = 0.0f; // used for clamping camera angle
    float velocityY = 0.0f;

    bool lockCursor = true; // hides & centres cursor

    //below is used for smoothing player & camera movement
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    CharacterController controller = null;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
        Jump();

    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * (currentMouseDelta.x * mouseSensitivity));

    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

       

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            Debug.Log("Player Grounded");
        }

        
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            velocityY += jumpSpeed;
            Debug.Log("Jumped with velocity of " + velocityY);
        }
       

        if (controller.isGrounded == false)
        {
            velocityY += gravity * Time.deltaTime;
            Debug.Log("Gravity is now being applied " + velocityY);
        } else
        {
            velocityY = 0.0f;
            Debug.Log("Normal VelocityY " + velocityY);
        }
    }

}