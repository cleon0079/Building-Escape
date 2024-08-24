using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragObject : MonoBehaviour
{
    [SerializeField] float rayDistance = 10f;
    [SerializeField] LayerMask draggableLayer;
    [SerializeField] float minHeight = 0.5f;
    float positionSpring = 200f;
    float positionDamper = 20f;
    float rotationSpring = 200f;
    float rotationDamper = 20f;
    float linearLimit = 0.5f;
    float angularLimit = 45f;
    [SerializeField] float minDistance = 1f;
    [SerializeField] float maxDistance = 10f;
    [SerializeField] float scrollSpeed = 1f;
    [SerializeField] float rotationSpeed = 100f;

    private Rigidbody draggedRigidbody;
    private ConfigurableJoint configurableJoint;
    private Transform grabTransform;
    private bool isDragging = false;
    private bool isRotating = false;
    private bool canRotate = false;
    private float currentDistance;

    private GameInput input;
    private InputAction interatAction;
    private InputAction scrollAction;
    private InputAction rotateAction;
    private InputAction deltaAction;
    private bool isRightClicking = false;

    private Controller player;

    private void Awake()
    {
        input = new GameInput();
        interatAction = input.Player.Interart;
        scrollAction = input.Player.Scroll;
        rotateAction = input.Player.Rotate;
        deltaAction = input.Player.Look;

        interatAction.started += OnDrag;
        scrollAction.performed += OnScroll;
        rotateAction.started += OnRightClick;

        player = FindObjectOfType<Controller>();

        grabTransform = new GameObject("Grab Point").transform;
        grabTransform.gameObject.AddComponent<Rigidbody>().isKinematic = true;
    }

    private void OnEnable()
    {
        interatAction.Enable();
        scrollAction.Enable();
        rotateAction.Enable();
        deltaAction.Enable();
    }

    private void OnDisable()
    {
        interatAction.Disable();
        scrollAction.Disable();
        rotateAction.Disable();
        deltaAction.Disable();
    }

    void OnRightClick(InputAction.CallbackContext context) {
        if (canRotate)
        {
            isRightClicking = !isRightClicking;
        }
        if (isRightClicking)
        {
            player.CanMove(false);
        }
        else
        {
            player.CanMove(true);
        }
    }    

    private void OnDrag(InputAction.CallbackContext context)
    {
        if (isDragging)
        {
            StopDrag();
        }
        else
        {
            TryStartDrag();
        }
    }

    void OnScroll(InputAction.CallbackContext context)
    {
        float scroll = context.ReadValue<Vector2>().y;
        AdjustGrabDistance(scroll);
    }

    void Update()
    {

        if (isDragging && draggedRigidbody != null)
        {
            DragTheObject();

            if (canRotate && isRightClicking)
            {
                Vector2 mouseDelta = deltaAction.ReadValue<Vector2>();
                RotateObject(mouseDelta);
            }
        }
    }

    void TryStartDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, draggableLayer))
        {
            draggedRigidbody = hit.rigidbody;
            if (draggedRigidbody != null)
            {
                grabTransform.position = hit.point;
                grabTransform.parent = null;

                currentDistance = Vector3.Distance(grabTransform.position, Camera.main.transform.position);

                AddConfigurableJoint();

                isDragging = true;
                canRotate = false;
            }
        }
    }

    void DragTheObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Vector3 targetPoint;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (((1 << hit.collider.gameObject.layer) & draggableLayer) != 0)
            {
                return;
            }
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * currentDistance;
        }

        targetPoint.y = Mathf.Max(targetPoint.y, minHeight);

        grabTransform.position = targetPoint;

        if (Vector3.Distance(grabTransform.position, draggedRigidbody.position) < 1)
        {
            canRotate = true;
        }
    }

    void StopDrag()
    {
        if (draggedRigidbody != null)
        {
            Destroy(configurableJoint);
            draggedRigidbody.useGravity = true;
            draggedRigidbody = null;
            isDragging = false;
            canRotate = false;
            isRightClicking = false;
            player.CanMove(true);
        }
    }

    void AdjustGrabDistance(float scroll)
    {
        if (isDragging && draggedRigidbody != null)
        {
            Vector3 direction = (grabTransform.position - Camera.main.transform.position).normalized;
            currentDistance = Mathf.Clamp(currentDistance + scroll * scrollSpeed, minDistance, maxDistance);

            grabTransform.position = Camera.main.transform.position + direction * currentDistance;
        }
    }

    void RotateObject(Vector2 mouseDelta)
    {
        if (draggedRigidbody == null) return;

        Vector3 rotationAxis = Camera.main.transform.up * -mouseDelta.x + Camera.main.transform.right * mouseDelta.y;

        if (rotationAxis != Vector3.zero)
        {
            if (!isRotating)
            {
                RemoveConfigurableJoint();
                isRotating = true;
            }

            draggedRigidbody.transform.RotateAround(draggedRigidbody.position, rotationAxis, rotationSpeed * Time.deltaTime);
        }
        else if (isRotating)
        {
            AddConfigurableJoint();
            isRotating = false;
        }
    }

    void AddConfigurableJoint()
    {
        if (draggedRigidbody == null) return;

        configurableJoint = draggedRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        configurableJoint.connectedBody = grabTransform.GetComponent<Rigidbody>();

        SoftJointLimit linearSoftLimit = new SoftJointLimit { limit = linearLimit };
        configurableJoint.linearLimit = linearSoftLimit;

        SoftJointLimitSpring angularLimitSpring = new SoftJointLimitSpring
        {
            spring = rotationSpring,
            damper = rotationDamper
        };
        configurableJoint.angularXLimitSpring = angularLimitSpring;
        configurableJoint.angularYZLimitSpring = angularLimitSpring;

        SoftJointLimit highAngularXLimit = new SoftJointLimit { limit = angularLimit };
        SoftJointLimit lowAngularXLimit = new SoftJointLimit { limit = -angularLimit };
        SoftJointLimit angularYZLimit = new SoftJointLimit { limit = angularLimit };
        configurableJoint.highAngularXLimit = highAngularXLimit;
        configurableJoint.lowAngularXLimit = lowAngularXLimit;
        configurableJoint.angularYLimit = angularYZLimit;
        configurableJoint.angularZLimit = angularYZLimit;

        JointDrive positionDrive = new JointDrive
        {
            positionSpring = positionSpring,
            positionDamper = positionDamper,
            maximumForce = float.MaxValue
        };
        configurableJoint.xDrive = positionDrive;
        configurableJoint.yDrive = positionDrive;
        configurableJoint.zDrive = positionDrive;

        JointDrive rotationDrive = new JointDrive
        {
            positionSpring = rotationSpring,
            positionDamper = rotationDamper,
            maximumForce = float.MaxValue
        };
        configurableJoint.angularXDrive = rotationDrive;
        configurableJoint.angularYZDrive = rotationDrive;

        draggedRigidbody.useGravity = false;
    }

    void RemoveConfigurableJoint()
    {
        if (draggedRigidbody == null) return;
        Destroy(configurableJoint);
    }
}
