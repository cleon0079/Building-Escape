using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NumBox : MonoBehaviour
{
    public int index = 0;

    public int originalX = 0; // Record original x position
    public int originalY = 0; // Record original y position

    int x = 0;
    int y = 0;

    private Action<int, int> swapFunc = null;

    public void Init(int i, int j, int index, Sprite sprite, Action<int, int> swapFunc)
    {
        this.index = index;

        this.originalX = i; // Record original x position
        this.originalY = j; // Record original y position

        this.GetComponent<SpriteRenderer>().sprite = sprite;
        UpdatePos(i, j);
        this.swapFunc = swapFunc;
    }

    public void UpdatePos(int i, int j)
    {
        x = i;
        y = j;
        this.gameObject.transform.localPosition = new Vector2(i, j);
    }

    public bool IsEmpty()
    {
        return index == 16;
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButton(0) && swapFunc != null)
        {
            swapFunc(x, y);
        }
    }
}


