using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class PlayerScript : MonoBehaviour
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
    public bool InputsEnabled = true;
    public GameObject pdaEquipObj;
    public GameObject flashLight;
    public GameObject heldObj;
    public int metalCount;
    public int plasticCount;
    public int ammoCount;
    public GameObject pdaObj;

    private Rigidbody rb;
    private Camera playerCamera;
    private float verticalLookRotation = 0f;
    public float horizontalLookRotation;
    private CapsuleCollider playerCollider;
    private float standHeight;
    private float camStandHeight;
    private bool unCrouch;
    private GameObject grabbedObj;
    private float startDrag;
    private bool CanMoveMouseInp;
    private bool pdaFlop;
    private EquipPda equipPdaScript;
    


    void Start()
    {
        GameManager.Instance.player = gameObject;
        //Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;
        //playerCamera.transform.parent = null;
        playerCollider = GetComponent<CapsuleCollider>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CanMoveMouseInp = true;
        }
        standHeight = playerCollider.height;
        camStandHeight = camAnch.transform.localPosition.y;
        moveSpeed = walkSpeed;
        startDrag = rb.drag;
        equipPdaScript = pdaEquipObj.GetComponent<EquipPda>();
    }

    void Update()
    {
        
        if (InputsEnabled)
        {
            if (CanMoveMouseInp) HandleMouseLook();

            Inputs();
        }
        
        
        //ObjectCarry();

        Drag();
        //OnDrawGizmos();

        //Application.targetFrameRate = setFrameRate;

        //if (Input.GetKeyDown(KeyCode.F)) rb.AddForce(new Vector3 (100f, 10f, 0f), ForceMode.Impulse);
        //print(IsGrounded());
    }

    private void Awake()
    {
        
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

    void Inputs()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonDown("Crouch"))
        {
            Crouch();
        }
        if (!Input.GetButton("Crouch"))
        {
            StandFromCrouch();
        }

        if (Input.GetButtonDown("Sprint"))
        {
            Sprint(true);
        }
        if (Input.GetButtonUp("Sprint"))
        {
            Sprint(false);
        }
        if (Input.GetButtonDown("Interact"))
        {
            Interact();
        }
        if (Input.GetButtonDown("TacScreen"))
        {
            OpenPda();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            FlashLight();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //SceneManager.LoadScene("MainMenu");
            Application.Quit();
        }
    }

    public void ScreenMouse(bool shouldMouse)
    {
        if (shouldMouse)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            CanMoveMouseInp = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CanMoveMouseInp = true;
        }
    }

    void Drag()
    {
        if (IsGrounded()) rb.drag = startDrag;
        else rb.drag = airDrag;
    }

    void Jump()
    {
        if (IsGrounded() && !crouched)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    bool IsGrounded()
    {
        //return Physics.Raycast(transform.position, Vector3.down, playerCollider.height / 2 + 0.1f);

        Vector3 spherePosition = transform.position + Vector3.down * (standHeight - crouchHeight - 0.2f);
        //Gizmos.DrawWireSphere(spherePosition, playerCollider.radius);
        return Physics.CheckSphere(spherePosition, playerCollider.radius, collisionLayers);
    }

    bool CanStand()
    {
        Vector3 spherePosition = transform.position + Vector3.up * (standHeight - crouchHeight);
        return !Physics.CheckSphere(spherePosition, playerCollider.radius, collisionLayers);
    }

    public void Crouch()
    {
        if (!crouched)
        {
            crouched = true;

            playerCollider.height = crouchHeight;
            transform.position = new Vector3(transform.position.x, transform.position.y - standHeight / 2 + crouchHeight / 2, transform.position.z);
            camAnch.transform.localPosition = new Vector3(camAnch.transform.localPosition.x, camCrouchHeight, camAnch.transform.localPosition.z);
            moveSpeed = crouchSpeed;
        }
        
    }

    public void StandFromCrouch()
    {
        if (crouched && CanStand())
        {
            crouched = false;
            
            playerCollider.height = standHeight;
            camAnch.transform.localPosition = new Vector3(camAnch.transform.localPosition.x, camStandHeight, camAnch.transform.localPosition.z);
            moveSpeed = walkSpeed;
        }
    }

    void Sprint(bool sprintInput)
    {
        if (!crouched)
        {
            if (sprintInput)
            {
                moveSpeed = sprintSpeed;
            }
            else
            {
                moveSpeed = walkSpeed;
            }
        }
    }


    void ObjectCarry()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            //Debug.Log("Hit object: " + hit.collider.gameObject.name);
            if (Input.GetButton("Grab") && hit.collider.gameObject.GetComponent<Rigidbody>() != null)
            {
                grabbedObj = hit.collider.gameObject;
                hit.collider.gameObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward * interactRange;
                hit.collider.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red);
    }

    void Interact()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            //print(hit.collider.gameObject);
            if (hit.collider.gameObject.GetComponent<InteractScript>() != null)
            {
                hit.collider.gameObject.GetComponent<InteractScript>().SendInteract();
            }
            
        }
    }

    //void OnDrawGizmos()
    //{
    //    //Vector3 spherePosition = transform.position + Vector3.up * (standHeight - crouchHeight);
    //    //Gizmos.color = CanStand() ? Color.green : Color.red;
    //    //Gizmos.DrawWireSphere(spherePosition, playerCollider.radius);

    //    //Vector3 spherePosition = transform.position + Vector3.down * (standHeight - crouchHeight - 0.2f);
    //    //Gizmos.DrawWireSphere(spherePosition, playerCollider.radius);
    //}

    void OpenPda()
    {
        if (pdaFlop == false)
        {
            pdaFlop = true;
            ScreenMouse(true);
            equipPdaScript.ActivatePda(true);
        }
        else
        {
            pdaFlop = false;
            ScreenMouse(false);
            equipPdaScript.ActivatePda(false);
        }
    }

    public void Death()
    {
        //SceneManager.LoadScene("MainMenu");
        Application.Quit();
        //Destroy(gameObject);
    }

    void FlashLight()
    {
        if (flashLight.activeSelf)
        {
            flashLight.SetActive(false);
        }
        else
        {
            flashLight.SetActive(true);
        }
    }
}
