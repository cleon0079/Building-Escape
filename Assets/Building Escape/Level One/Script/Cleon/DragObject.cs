using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    [SerializeField] float rayDistance = 10f; // 射线检测距离
    [SerializeField] LayerMask draggableLayer; // 可拖动物体的层级
    [SerializeField] float minHeight = 0.5f; // 物体的最小高度，防止穿过平面
    float positionSpring = 200f; // 位置弹簧
    float positionDamper = 20f; // 位置阻尼
    float rotationSpring = 200f; // 旋转弹簧
    float rotationDamper = 20f; // 旋转阻尼
    float linearLimit = 0.5f; // 线性限制
    float angularLimit = 45f; // 角度限制
    [SerializeField] float minDistance = 1f; // 物体离玩家的最小距离
    [SerializeField] float maxDistance = 10f; // 物体离玩家的最大距离
    [SerializeField] float scrollSpeed = 1f; // 滚轮滚动的速度
    [SerializeField] float rotationSpeed = 100f; // 旋转速度

    private Rigidbody draggedRigidbody; // 当前被拖动的物体的Rigidbody
    private ConfigurableJoint configurableJoint; // 用于抓取物体的可配置关节
    private Transform grabTransform; // 抓取点的虚拟Transform
    private bool isDragging = false; // 当前是否在拖拽物体
    private bool isRotating = false; // 当前是否在旋转物体
    private bool canRotate = false; // 是否可以旋转物体
    private float currentDistance; // 当前抓取点与玩家的距离

    void Start()
    {
        // 创建一个虚拟的抓取点
        grabTransform = new GameObject("Grab Point").transform;
        grabTransform.gameObject.AddComponent<Rigidbody>().isKinematic = true;
    }

    void Update()
    {
        // 检测鼠标左键或F键按下，切换拖拽状态
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

        // 如果正在拖拽物体，更新拖拽逻辑
        if (isDragging && draggedRigidbody != null)
        {
            DragTheObject();

            // 检测鼠标滚轮滚动
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                AdjustGrabDistance(scroll);
            }

            // 检测WASD键，旋转物体
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

                // 设置拖拽状态为true
                isDragging = true;
                canRotate = false; // 初始化时不允许旋转
            }
        }
    }

    void DragTheObject()
    {
        // 更新抓取点位置
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Vector3 targetPoint;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (((1 << hit.collider.gameObject.layer) & draggableLayer) != 0)
            {
                return; // 如果检测到的物体属于停止移动层级，则不移动抓取点
            }
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * currentDistance;
        }

        // 确保抓取点的目标高度不会低于minHeight
        targetPoint.y = Mathf.Max(targetPoint.y, minHeight);

        grabTransform.position = targetPoint;

        // 检查物体是否到达目标位置
        if (Vector3.Distance(grabTransform.position, draggedRigidbody.position) < 1)
        {
            canRotate = true;
        }
    }

    void StopDrag()
    {
        // 结束拖动，销毁关节并重置拖拽状态
        if (draggedRigidbody != null)
        {
            Destroy(configurableJoint);
            draggedRigidbody.useGravity = true; // 恢复重力
            draggedRigidbody = null;
            isDragging = false;
            canRotate = false; // 停止拖拽时不允许旋转
        }
    }

    void AdjustGrabDistance(float scroll)
    {
        // 获取当前抓取点的方向和距离
        Vector3 direction = (grabTransform.position - Camera.main.transform.position).normalized;
        currentDistance = Mathf.Clamp(currentDistance + scroll * scrollSpeed, minDistance, maxDistance);

        // 根据新的距离调整抓取点位置
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

            // 进行旋转
            draggedRigidbody.transform.RotateAround(draggedRigidbody.position, rotationAxis, rotationSpeed * Time.deltaTime);
        }
        else if (isRotating)
        {
            // 恢复关节和重力
            AddConfigurableJoint();
            isRotating = false;
        }
    }

    void AddConfigurableJoint()
    {
        if (draggedRigidbody == null) return;

        configurableJoint = draggedRigidbody.gameObject.AddComponent<ConfigurableJoint>();
        configurableJoint.connectedBody = grabTransform.GetComponent<Rigidbody>();

        // 设置线性和角度限制
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

        // 配置位置和角度驱动
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
