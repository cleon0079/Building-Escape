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
            other.transform.SetParent(this.transform);
            other.transform.DOLocalMove(new Vector3(0, 0, -1), 1f);
            other.transform.DOLocalRotate(Vector3.zero, 1f);

            dragObject.StopDrag();
            isChair = true;
        }

        if (other.transform.CompareTag("Table") && !isTable)
        {
            other.transform.SetParent(this.transform);
            other.transform.DOLocalMove(Vector3.zero, 1f);
            other.transform.DOLocalRotate(Vector3.zero, 1f);

            dragObject.StopDrag();
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
