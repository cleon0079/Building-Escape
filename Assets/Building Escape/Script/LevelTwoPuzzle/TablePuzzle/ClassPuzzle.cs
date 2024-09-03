using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassPuzzle : MonoBehaviour
{
    DragObject dragObject;
    bool isChair = false;
    bool isTable = false;

    void Start()
    {
        dragObject = FindObjectOfType<DragObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Chair") && !isChair)
        {
            dragObject.StopDrag();
            other.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.SetParent(this.transform);

            other.transform.DOLocalMove(new Vector3(0, 0, -1.5f), 1f);
            other.transform.DOLocalRotate(Vector3.zero, 1f);

            isChair = true;
        }

        if (other.transform.CompareTag("Table") && !isTable)
        {
            dragObject.StopDrag();
            other.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.transform.SetParent(this.transform);

            other.transform.DOLocalMove(Vector3.zero, 1f);
            other.transform.DOLocalRotate(Vector3.zero, 1f);

            isTable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Chair") && isChair)
        {
            other.transform.SetParent(null);
            isChair = false;
        }

        if (other.transform.CompareTag("Table") && isTable)
        {
            other.transform.SetParent(null);
            isTable = false;
        }
    }
}
