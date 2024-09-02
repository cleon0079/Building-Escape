using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlackBoardPuzzleCheck : MonoBehaviour
{
    [SerializeField] Transform targetPosition;
    [SerializeField] GameObject prizeGB;
    GameObject[] blackboards;
    int index = 0;

    private void Start()
    {
        blackboards = new GameObject[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            blackboards[i] = this.transform.GetChild(i).gameObject;
        }
    }

    public void Check() {
        index = 0;
        for (int i = 0; i < blackboards.Length; i++)
        {
            if (blackboards[i].GetComponent<BlackBoardPuzzle>().GetPuzzle())
            {
                index++;
            }
        }
        Debug.Log(1);
        if (index == blackboards.Length)
        {
            Finish();
        }
    }

    void Finish() {
        prizeGB.SetActive(true);
        prizeGB.transform.DOMove(targetPosition.position, 2f);
        prizeGB.transform.DORotate(Vector3.zero, 2f);
    }
}
