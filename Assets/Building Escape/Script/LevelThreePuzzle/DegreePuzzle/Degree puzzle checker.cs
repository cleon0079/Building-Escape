using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Degreepuzzlechecker : MonoBehaviour
{
    [SerializeField] Transform targetPosition;
    GameObject[] degreeBoards;
    // Manager uimanager;
    int index = 0;
    public bool isFinish;
    bool canTrigger = true;

    // Start is called before the first frame update
    void Start()
    {
        // uimanager = FindObjectOfType<Manager>();
        isFinish = false;
        degreeBoards = new GameObject[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            degreeBoards[i] = this.transform.GetChild(i).GetChild(0).gameObject;
            degreeBoards[i].GetComponent<BlackBoardPuzzle>().SetIndex(i + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Check() {
        index = 0;
        for (int i = 0; i < degreeBoards.Length; i++)
        {
            if (degreeBoards[i].GetComponent<BlackBoardPuzzle>().GetPuzzle())
            {
                index++;
            }
        }
        if (index == degreeBoards.Length)
        {
            Finish();
        }
    }
    void Finish()
    {
        isFinish = true;
    }
    public bool GetTrigger() {
        return canTrigger;
    }
    public void SetTrigger(bool trigger) {
        this.canTrigger = trigger;
    }

    public GameObject[] GetTriggerList() {
        return degreeBoards;
    }
}
