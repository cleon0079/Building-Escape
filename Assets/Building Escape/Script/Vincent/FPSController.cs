using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{   
    //camera
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private Camera LockCamera;

    public float walkSpeed =6f;
    public float runSpeed = 12f;

    public float lookSpeed = 2f;
    public float lookXLimit = 90f;  
    public float jumpPower= 50f;
    public float gravity = 50f;

    private bool onGround; 

    Vector3 moveDirection = Vector3.zero;
    private float rotationX =0;
    public bool canMove = true;
    private bool IsTrigger =false;

    private GameManager gm;
    [SerializeField] private Camera numLockCamera;
    private bool isNumLock = false;

    private CharacterController characterController;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //
        LockCamera.enabled = false;
        numLockCamera.enabled = false;
    }

    // Update is called once per frame
    void Update(){
        
            // Handles Movement
    Vector3 forward = transform.TransformDirection(Vector3.forward);
    Vector3 right = transform.TransformDirection(Vector3.right);

    // Press Left Shift to run
    bool isRunning = Input.GetKey(KeyCode.LeftShift);
    float speed = isRunning ? runSpeed : walkSpeed;
    float moveSpeed = canMove ? speed : 0;

    float moveX = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
    float moveZ = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

    Vector3 movement = forward * moveX + right * moveZ;

        if (gm.GetZoom() == false)
        {
            // Handles Rotation
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            PlayerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        }

        // Apply gravity
        if (characterController.isGrounded)
    {
        moveDirection.y = 0f;
        onGround = true;
    }
    else
    {
        moveDirection.y -= gravity * Time.deltaTime;
        onGround = false;
    }

    // Jumping
    if (canMove && Input.GetButtonDown("Jump"))
    {
        if (onGround || isRunning) // Allow jumping if on the ground or running
        {
            moveDirection.y = jumpPower;
            onGround = false;
        }
    }

    // Move the character controller
    characterController.Move(movement + moveDirection * Time.deltaTime);

    HandleTrigger();
    }

    void HandleTrigger(){
        // Check trigger the puzzle
        if (IsTrigger ==true  && Input.GetKeyDown(KeyCode.E)){
            // Disable the main camera
            PlayerCamera.enabled = false;
            // Enable the Lock camera
            LockCamera.enabled = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        //Excape the puzzle
        if (Input.GetKeyDown(KeyCode.Escape)){
            PlayerCamera.enabled = true;
            // Enable the Lock camera
            LockCamera.enabled = false;
            numLockCamera.enabled = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


            gm.numLock.CloseNumLock();
        }

        if (isNumLock && Input.GetKeyDown(KeyCode.E))
        {
            // Disable the main camera
            PlayerCamera.enabled = false;
            // Enable the Lock camera
            numLockCamera.enabled = true;

            gm.numLock.OpenNumLock();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void OnTriggerEnter(Collider  other){
        if (other.CompareTag("Lock")){
            Debug.Log("Player trigger Enter" + other);
            IsTrigger =true;
        }

        if (other.CompareTag("NumLock"))
        {
            isNumLock = true;
        }
    }

    public void OnTriggerExit(Collider  other){
        if (other.CompareTag("Lock")){
            Debug.Log("Player exited " + other);
            IsTrigger =false;
        }

        if (other.CompareTag("NumLock"))
        {
            isNumLock = false;
        }
    }


}
