using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Item item;
    private GameManager gameManager;
    Rigidbody rigidbody;
    

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (gameManager.isZoom)
        {
            if (Input.GetMouseButton(0))
            {
                transform.Rotate(Vector3.up, -Input.GetAxis("Mouse X") * 10, Space.Self);
                transform.Rotate(Vector3.right, Input.GetAxis("Mouse Y") * 10, Space.Self);
            }
        }
        
    }

    public void GetItem() {
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
    }

    public void Drop() {
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
    }
}