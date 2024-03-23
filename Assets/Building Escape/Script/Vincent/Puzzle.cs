using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{   
    public NumBox boxPrefab;

    public NumBox[,] boxes= new NumBox[4,4];

    public Sprite[] sprites;

    private int emptyIndex = 16;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Shuffle();
    }

    void Init()
    {
        int n = 0;
        for (int y = 3; y >= 0; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                NumBox box = Instantiate(boxPrefab, new Vector2(x, y), Quaternion.identity);
                box.Init(x, y, n + 1, sprites[n], ClickToSwap);
                boxes[x, y] = box;
                n++;
            }
        }
    }

    void ClickToSwap(int x, int y)
    {
        int dx = GetDx(x, y);
        int dy = GetDy(x, y);

        var selectTarget = boxes[x, y];
        var chargeTarget = boxes[x + dx, y + dy];

        // Swap the boxes
        boxes[x, y] = chargeTarget;
        boxes[x + dx, y + dy] = selectTarget;

        selectTarget.UpdatePos(x + dx, y + dy);
        chargeTarget.UpdatePos(x, y);

        CheckComplete(); // Check for completion after each swap
    }

    int GetDx(int x, int y)
    {
        if (x < 3 && boxes[x + 1, y].IsEmpty())
        {
            return 1;
        }
        if (x > 0 && boxes[x - 1, y].IsEmpty())
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    int GetDy(int x, int y)
    {
        if (y < 3 && boxes[x, y + 1].IsEmpty())
        {
            return 1;
        }
        if (y > 0 && boxes[x, y - 1].IsEmpty())
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    // Shuffle the puzzle
    void Shuffle()
    {
        for (int i = 0; i < 100; i++)
        {
            int randomX = Random.Range(0, 4);
            int randomY = Random.Range(0, 4);
            if (!boxes[randomX, randomY].IsEmpty())
            {
                ClickToSwap(randomX, randomY);
            }
        }
    }

    // Check if the puzzle is completed
    void CheckComplete()
    {
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (boxes[x, y].originalX != x || boxes[x, y].originalY != y)
                {
                    Debug.Log("Puzzle is not complete yet!");
                    return;
                }
            }
        }
        Debug.Log("Puzzle is complete!");
    }
}
