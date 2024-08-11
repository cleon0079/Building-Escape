using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    [SerializeField] float rayDistance = 10f; // ���߼�����
    [SerializeField] LayerMask draggableLayer; // ���϶�����Ĳ㼶
    [SerializeField] float minHeight = 0.5f; // �������С�߶ȣ���ֹ����ƽ��
    float positionSpring = 200f; // λ�õ���
    float positionDamper = 20f; // λ������
    float rotationSpring = 200f; // ��ת����
    float rotationDamper = 20f; // ��ת����
    float linearLimit = 0.5f; // ��������
    float angularLimit = 45f; // �Ƕ�����
    [SerializeField] float minDistance = 1f; // ��������ҵ���С����
    [SerializeField] float maxDistance = 10f; // ��������ҵ�������
    [SerializeField] float scrollSpeed = 1f; // ���ֹ������ٶ�
    [SerializeField] float rotationSpeed = 100f; // ��ת�ٶ�

    private Rigidbody draggedRigidbody; // ��ǰ���϶��������Rigidbody
    private ConfigurableJoint configurableJoint; // ����ץȡ����Ŀ����ùؽ�
    private Transform grabTransform; // ץȡ�������Transform
    private bool isDragging = false; // ��ǰ�Ƿ�����ק����
    private bool isRotating = false; // ��ǰ�Ƿ�����ת����
    private bool canRotate = false; // �Ƿ������ת����
    private float currentDistance; // ��ǰץȡ������ҵľ���

    void Start()
    {
        // ����һ�������ץȡ��
        grabTransform = new GameObject("Grab Point").transform;
        grabTransform.gameObject.AddComponent<Rigidbody>().isKinematic = true;
    }

    void Update()
    {
        // �����������F�����£��л���ק״̬
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
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

        // ���������ק���壬������ק�߼�
        if (isDragging && draggedRigidbody != null)
        {
            DragTheObject();

            // ��������ֹ���
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                AdjustGrabDistance(scroll);
            }

            // ���WASD������ת����
            if (canRotate)
            {
                RotateObject();
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

                // ������ק״̬Ϊtrue
                isDragging = true;
                canRotate = false; // ��ʼ��ʱ��������ת
            }
        }
    }

    void DragTheObject()
    {
        // ����ץȡ��λ��
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Vector3 targetPoint;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (((1 << hit.collider.gameObject.layer) & draggableLayer) != 0)
            {
                return; // �����⵽����������ֹͣ�ƶ��㼶�����ƶ�ץȡ��
            }
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * currentDistance;
        }

        // ȷ��ץȡ���Ŀ��߶Ȳ������minHeight
        targetPoint.y = Mathf.Max(targetPoint.y, minHeight);

        grabTransform.position = targetPoint;

        // ��������Ƿ񵽴�Ŀ��λ��
        if (Vector3.Distance(grabTransform.position, draggedRigidbody.position) < 1)
        {
            canRotate = true;
        }
    }

    void StopDrag()
    {
        // �����϶������ٹؽڲ�������ק״̬
        if (draggedRigidbody != null)
        {
            Destroy(configurableJoint);
            draggedRigidbody.useGravity = true; // �ָ�����
            draggedRigidbody = null;
            isDragging = false;
            canRotate = false; // ֹͣ��קʱ��������ת
        }
    }

    void AdjustGrabDistance(float scroll)
    {
        // ��ȡ��ǰץȡ��ķ���;���
        Vector3 direction = (grabTransform.position - Camera.main.transform.position).normalized;
        currentDistance = Mathf.Clamp(currentDistance + scroll * scrollSpeed, minDistance, maxDistance);

        // �����µľ������ץȡ��λ��
        grabTransform.position = Camera.main.transform.position + direction * currentDistance;
    }

    void RotateObject()
    {
        if (draggedRigidbody == null) return;

        Vector3 rotationAxis = Vector3.zero;

        if (Input.GetKey(KeyCode.I))
        {
            rotationAxis = Camera.main.transform.right;
        }
        if (Input.GetKey(KeyCode.K))
        {
            rotationAxis = -Camera.main.transform.right;
        }
        if (Input.GetKey(KeyCode.J))
        {
            rotationAxis = Camera.main.transform.forward;
        }
        if (Input.GetKey(KeyCode.L))
        {
            rotationAxis = -Camera.main.transform.forward;
        }

        if (rotationAxis != Vector3.zero)
        {
            if (!isRotating)
            {
                RemoveConfigurableJoint();
                isRotating = true;
            }

            // ������ת
            draggedRigidbody.transform.RotateAround(draggedRigidbody.position, rotationAxis, rotationSpeed * Time.deltaTime);
        }
        else if (isRotating)
        {
            // �ָ��ؽں�����
            AddConfigurableJoint();
            isRotating = false;
        }
    }

    void AddConfigurableJoint()
    {
        if (draggedRigidbody == null) return;

        configurableJoint = draggedRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        configurableJoint.connectedBody = grabTransform.GetComponent<Rigidbody>();

        // �������ԺͽǶ�����
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

        // ����λ�úͽǶ�����
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
