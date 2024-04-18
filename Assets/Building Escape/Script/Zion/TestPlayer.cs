
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class TestPlayer : MonoBehaviour
{
    //camera
    [SerializeField] private Camera PlayerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;

    public float lookSpeed = 2f;
    public float lookXLimit = 90f;
    public float jumpPower = 50f;
    public float gravity = 50f;

    private bool onGround;

    Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    public bool canMove = true;
    private bool IsTrigger = false;



    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {

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

        // Handles Rotation
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        PlayerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

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

    }

}
