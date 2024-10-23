using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoPuzzleItem : MonoBehaviour
{
    private float power;
    Rigidbody rigidbody;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        power = 3f;
    }

    public void Push(Vector3 direction) {
        rigidbody.AddForce(transform.TransformDirection(direction) * power, ForceMode.Impulse);
    }
}
