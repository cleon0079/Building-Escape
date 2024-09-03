using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TablePuzzleCheck : MonoBehaviour
{
    GameObject[] tables;
    int index = 0;

    [SerializeField] Transform targetPosition;
    [SerializeField] GameObject prizeGB;

    // Start is called before the first frame update
    void Start()
    {
        tables = new GameObject[this.transform.childCount];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            tables[i] = this.transform.GetChild(i).gameObject;
            tables[i].GetComponent<TablePuzzle>().SetIndex(i + 1);
        }
    }
    public void Check()
    {
        index = 0;
        for (int i = 0; i < tables.Length; i++)
        {
            if (tables[i].GetComponent<TablePuzzle>().GetPuzzle())
            {
                index++;
            }
        }
        if (index == tables.Length)
        {
            Finish();
        }
    }

    void Finish()
    {
        prizeGB.SetActive(true);
        prizeGB.transform.DOMove(targetPosition.position, 2f);
        prizeGB.transform.DORotate(Vector3.zero, 2f);
    }
}
