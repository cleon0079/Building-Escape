using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PaperPuzzlePlayerIn : MonoBehaviour
{
    bool isPuzzling = false;
    int finishIndex = 0;

    [SerializeField] string showedText = "Press F to interat";
    [SerializeField] GameObject target;

    private GameInput input;
    private InputAction interatAction;
    private InputAction escAction;
    private InputAction mousePosition;

    Manager uiManager;

    private void Awake()
    {
        input = new GameInput();
        interatAction = input.Player.Interart;
        escAction = input.Player.Esc;
        mousePosition = input.Player.MousePosition;

        interatAction.started += OnStart;
    }

    void OnStart(InputAction.CallbackContext context)
    {
        uiManager.StartPuzzle();


    }

    private void Start()
    {
        uiManager = FindObjectOfType<Manager>();
    }

    public void Done() {
        finishIndex++;
        if (finishIndex == 3)
        {
            // TODO
            Debug.Log("Done");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!isPuzzling)
            {
                uiManager.UpdateText(showedText);
            }
            interatAction.Enable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            uiManager.UpdateText("");
            interatAction.Disable();
        }
    }
}
