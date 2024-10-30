using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Degreepuzzlechecker : MonoBehaviour
{
    [SerializeField] Transform targetPosition;
    [SerializeField] GameObject prizeGB;
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
            degreeBoards[i].GetComponent<DegreePuzzle2>().SetIndex(i + 1);
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
            if (degreeBoards[i].GetComponent<DegreePuzzle2>().GetPuzzle())
            {
                index++;
            }
        }
        Debug.Log("index: "+index);
        Debug.Log("Degree length: "+degreeBoards.Length);
        if (index == degreeBoards.Length)
        {
            Finish();
        }
    }
    void Finish()
    {
        isFinish = true;
        prizeGB.SetActive(true);
        prizeGB.transform.DOMove(targetPosition.position, 2f);
        prizeGB.transform.DORotate(Vector3.zero, 2f);
        Debug.Log("is finished");
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
