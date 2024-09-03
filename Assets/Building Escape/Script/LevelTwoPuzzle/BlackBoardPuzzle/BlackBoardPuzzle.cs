using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoardPuzzle : MonoBehaviour
{
    DragObject dragObject;
    BlackBoardPuzzleCheck puzzleCheck;
    bool isPuzzleIn = false;

    bool startCount = false;
    float timer = 0;

    [SerializeField] int index;
    bool isRight = false;

    private void Start()
    {
        dragObject = FindObjectOfType<DragObject>();
        puzzleCheck = this.transform.parent.GetComponent<BlackBoardPuzzleCheck>();
    }

    private void Update()
    {
        if (startCount)
        {
            timer += Time.deltaTime;
            if (timer >= 2f)
            {
                timer = 0;
                startCount = false;
                puzzleCheck.SetTrigger(true);
                isPuzzleIn = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("BlackBoard") && !isPuzzleIn && puzzleCheck.GetTrigger() && !isRight)
        {
            other.transform.SetParent(this.transform.parent);  
            other.transform.DOLocalMove(this.transform.localPosition, 1f);
            other.transform.DOLocalRotate(Vector3.zero, 1f);

            dragObject.StopDrag();

            Destroy(other.GetComponent<Rigidbody>());
            isPuzzleIn = true;


            other.GetComponent<BlackBoardItem>().SetPuzzleIn(true);

            if (other.GetComponent<BlackBoardItem>().GetIndex() == index)
            {
                isRight = true;
                other.GetComponent<BlackBoardItem>().CantMove();
            }
            puzzleCheck.Check();
        }
    }

    public bool GetPuzzle() {
        return isRight;
    }

    public void SetCount(bool count) {
        this.startCount = count;
    }

    public void SetIndex(int index) {
        this.index = index;
    }
}
