using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
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

    public void StartGame(bool start) {
        if (start)
        {
            Cursor.lockState = CursorLockMode.None;
            dragObject = FindObjectOfType<DragObject>();
            player = FindObjectOfType<Controller>();
            dragObject.CanDrag(false);
            player.CanMove(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            dragObject = FindObjectOfType<DragObject>();
            player = FindObjectOfType<Controller>();
            dragObject.CanDrag(true);
            player.CanMove(true);
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
