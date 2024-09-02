using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour
{
    UIManager uiManager;

    private GameInput input;
    private InputAction move;
    private InputAction look;
    private InputAction jump;

    //Control player Move 
    [SerializeField] private float moveSpeed = 10f;

    //control player Look
    [SerializeField] private float lookSensitivity = 1f;
    private float yRotation;

    //Control player jump
    [SerializeField] private float jumpHight = 5f;
    private Rigidbody rb;
    private bool isGrounded;

    bool canMove = false;
    bool canRotate = false;

    private void Awake()
    {
        input = new GameInput();
        move = input.Player.Move;
        look = input.Player.Look;
        jump = input.Player.Jump;

        jump.started += Jump;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void OnEnable()
    {
        move.Enable();
        look.Enable();
        jump.Enable();
        canMove = true;
        canRotate = true;
    }

    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        jump.Disable();
        canMove = false;
        canRotate = false;
    }

    void Start()
    {
       uiManager = FindObjectOfType<UIManager>();
       uiManager.CursorMode(false);

    }

    void Update()
    {
        if (canMove)
        {
            Movement();
        }
        if (canRotate)
        {
            RotateCamera();
        }
        //check the player is ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, this.transform.localScale.y * 1.1f);
    }

    void Movement()
    {
        float vertcal = move.ReadValue<Vector2>().x;
        float hirzontal = move.ReadValue<Vector2>().y;

        //Walk
        Vector3 moveDirection = (transform.forward * hirzontal + transform.right * vertcal).normalized;
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        //transform.Translate(Vector3.forward * hirzontal * Time.deltaTime * moveSpeed);
        //Translate(Vector3.right * vertcal * Time.deltaTime * moveSpeed);
    }

    void RotateCamera()
    {
        float mouse_x = look.ReadValue<Vector2>().x;
        float mouse_y = look.ReadValue<Vector2>().y;

        // Horizontal rotation
        transform.Rotate(0f, mouse_x * lookSensitivity,0f);

        yRotation -= mouse_y * lookSensitivity;
        yRotation = Mathf.Clamp(yRotation, -90f, 90f);

        uiManager.mainCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpHight, ForceMode.Impulse);
        }
        
    }


    public void CanMove(bool move) {
        if (move)
        {
            OnEnable();
        }
        else
        {
            OnDisable();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * transform.localScale.y * 1.1f);
    }
}



