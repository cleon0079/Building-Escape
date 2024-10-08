using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoPuzzle : MonoBehaviour
{
    private Collider[] colliders;
    private Collider player;

    private bool isInArea = false;
    private bool isDone = false;
    private int puzzleNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Controller>().gameObject.GetComponent<Collider>();

        puzzleNum = this.transform.childCount - 2;
        colliders = new Collider[this.transform.childCount];

        for (int i = 0; i < this.transform.childCount; i++)
        {
            colliders[i] = this.transform.GetChild(i).GetComponent<Collider>();
        }

        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (i < puzzleNum)
            {
                for (int j = i + 1; j < puzzleNum; j++)
                {
                    Physics.IgnoreCollision(colliders[i], colliders[j]);
                }
            }
            else
            {
                Physics.IgnoreCollision(colliders[i], player);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isInArea && !isDone)
        {
            isDone = true;

            for (int i = 0; i < puzzleNum - 1; i++)
            {
                if (this.transform.GetChild(i).transform.localPosition.x >= this.transform.GetChild(i + 1).transform.localPosition.x)
                {
                    isDone = false;
                    break;
                }
            }
        }

        if (isDone)
        {
            Debug.Log(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isInArea = true;
        }
    }
}
