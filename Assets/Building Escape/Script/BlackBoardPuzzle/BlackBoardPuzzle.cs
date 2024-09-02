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
        if (other.transform.CompareTag("BlackBoard") && !isPuzzleIn && this.name.Equals(other.gameObject.name))
        {
            other.transform.SetParent(this.transform.parent);  
            other.transform.DOLocalMove(this.transform.localPosition, 1f);
            other.transform.DOLocalRotate(Vector3.zero, 1f);

            dragObject.StopDrag();
            Destroy(other.GetComponent<Rigidbody>());
            isPuzzleIn = true;

            this.transform.parent.GetComponent<BlackBoardPuzzleCheck>().Check();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("BlackBoard") && isPuzzleIn && this.name.Equals(other.gameObject.name))
        {
            other.transform.SetParent(null);
            isPuzzleIn = false;
        }
    }

    public bool GetPuzzle() {
        return isPuzzleIn;
    }
}
