using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechMovement : MonoBehaviour
{
    public GameObject camAnch;
    public GameObject directionObj;
    public float moveSpeed;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 2f;
    public float airMoveSpeed = 0.2f;
    public float lookSensitivity = 2f;
    public float jumpForce = 5f;
    public bool lockCursor = true;
    public float crouchHeight = 1f;
    public float camCrouchHeight = 0.25f;
    public bool crouched;
    public LayerMask collisionLayers;
    public float interactRange = 2f;
    public float camSmoothSpeed = 0.1f;
    public int setFrameRate = 60;
    public float airDrag = 2f;

    private Rigidbody rb;
    private Camera playerCamera;
    private float verticalLookRotation = 0f;
    private float horizontalLookRotation;
    private CapsuleCollider playerCollider;
    


    void Start()
    {
        //Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        //playerCamera.transform.parent = null;
        playerCollider = GetComponent<CapsuleCollider>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        moveSpeed = walkSpeed;

    }

    void Update()
    {


        HandleMouseLook();
        
        //OnDrawGizmos();

        //Application.targetFrameRate = setFrameRate;

        if (Input.GetKeyDown(KeyCode.F)) rb.AddForce(new Vector3(100f, 10f, 0f), ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void LateUpdate()
    {

    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = (directionObj.transform.right * moveX + directionObj.transform.forward * moveZ);
        if (move.magnitude > 1) move.Normalize();
        float moveMult;
        if (IsGrounded()) moveMult = 1f;
        else moveMult = airMoveSpeed;

        rb.AddForce(move * moveSpeed * moveMult, ForceMode.Acceleration);
        //rb.velocity = new Vector3(move.x * moveSpeed * moveMult, rb.velocity.y, move.z * moveSpeed * moveMult);
    }

    void HandleMouseLook()
    {
        //playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, camAnch.transform.position, camSmoothSpeed);
        //playerCamera.transform.position = camAnch.transform.position;         Cam move on player if no parent
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        horizontalLookRotation += mouseX;
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        directionObj.transform.rotation = Quaternion.Euler(0f, horizontalLookRotation, 0f);
        camAnch.transform.rotation = Quaternion.Euler(verticalLookRotation, horizontalLookRotation, 0f);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, playerCollider.height / 2 + 0.1f);
    }
}
