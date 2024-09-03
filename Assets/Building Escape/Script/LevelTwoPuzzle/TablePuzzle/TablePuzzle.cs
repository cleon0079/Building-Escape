using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablePuzzle : MonoBehaviour
{
    DragObject dragObject;
    bool isChair = false;
    bool isTable = false;

    [SerializeField] int index;
    bool isRight = false;

    void Start()
    {
        dragObject = FindObjectOfType<DragObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckChair(other);

        if (other.transform.CompareTag("Table") && !isTable && !isRight)
        {
            other.transform.SetParent(this.transform);

            other.transform.DOLocalMove(Vector3.zero, 1f);
            other.transform.DOLocalRotate(Vector3.zero, 1f);

            dragObject.StopDrag();

            isTable = true;

            other.GetComponent<TableItem>().SetTableIn(true);

            if (other.GetComponent<TableItem>().GetIndex() == index)
            {
                isRight = true;
                other.GetComponent<TableItem>().CantMove();
            }

            this.transform.parent.GetComponent<TablePuzzleCheck>().Check();
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

    void CheckChair(Collider other) {
        if (other.transform.CompareTag("Chair") && !isChair)
        {

            other.transform.SetParent(this.transform);

            other.transform.DOLocalMove(new Vector3(0, 0, -1.5f), 1f);
            other.transform.DOLocalRotate(Vector3.zero, 1f);

            dragObject.StopDrag();

            isChair = true;


        }
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public bool GetPuzzle()
    {
        return isRight;
    }
}
