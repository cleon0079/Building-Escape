using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interatText;
    [SerializeField] GameObject middelDot;
    public Camera mainCamera;
    DragObject dragObject;
    Controller player;

    private void Start()
    {
        dragObject = FindObjectOfType<DragObject>();
        player = FindObjectOfType<Controller>();
    }

    public void StartPuzzle()
    {
        dragObject.CanDrag(false);
        player.CanMove(false);
    }

    public void EndPuzzle() 
    {
        dragObject.CanDrag(true);
        player.CanMove(true);
    }

    public void ShowDot(bool dot) 
    {
        middelDot.SetActive(dot);
    }

    public void UpdateText(string inText)
    {
        interatText.text = inText;
    }

    public void Inventory(bool inventory) {
        dragObject.CanDrag(inventory);
        player.CanMove(inventory);
    }

    public void CursorMode(bool mode) 
    {
        if (mode)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
