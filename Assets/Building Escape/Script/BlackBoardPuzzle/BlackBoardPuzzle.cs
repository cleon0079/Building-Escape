using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoardPuzzle : MonoBehaviour
{
    DragObject dragObject;
    bool isPuzzleIn = false;

    private void Start()
    {
        dragObject = FindObjectOfType<DragObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("BlackBoard") && !isPuzzleIn)
        {
            other.transform.SetParent(this.transform.parent);
            other.transform.DOLocalMove(Vector3.zero, 1f);
            other.transform.DOLocalRotate(Vector3.zero, 1f);

            dragObject.StopDrag();
            Destroy(other.GetComponent<Rigidbody>());
            isPuzzleIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("BlackBoard") && isPuzzleIn)
        {
            other.transform.SetParent(null);
            isPuzzleIn = false;
        }
    }
}
