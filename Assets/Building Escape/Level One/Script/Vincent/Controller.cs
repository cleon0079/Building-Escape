using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{

    private GameInput input;
    private InputAction move;
    private InputAction look;
    private InputAction jump;


    //Control player Move 
    [SerializeField] private float moveSpeed = 10f;

    //control player Look
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float lookSensitivity = 10f;
    private float yRotation;

    //Control player jump
    [SerializeField] private float jumpHight = 1.0f;
    private Rigidbody rb;
    private bool isGrounded;

    private void Awake()
    {
        input = new GameInput();
        move = input.Player.Move;
        look = input.Player.Look;
        jump = input.Player.Jump;
        jump.started += Jump;

        rb = GetComponent<Rigidbody>();

    }

    private void OnEnable()
    {
        move.Enable();
        look.Enable();
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        jump.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        Movement();
        Camera();

        //check the player is ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);


    }

    void Movement()
    {
        float vertcal = move.ReadValue<Vector2>().x;
        float hirzontal = move.ReadValue<Vector2>().y;

        //Walk
        transform.Translate(Vector3.forward * hirzontal * Time.deltaTime * moveSpeed);
        transform.Translate(Vector3.right * vertcal * Time.deltaTime * moveSpeed);
    }

    void Camera()
    {
        float mouse_x = look.ReadValue<Vector2>().x;
        float mouse_y = look.ReadValue<Vector2>().y;

        // Horizontal rotation
        transform.Rotate(0f, mouse_x * lookSensitivity,0f);

        yRotation -= mouse_y * lookSensitivity;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        mainCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpHight, ForceMode.Impulse);
        }
        
    }


}



