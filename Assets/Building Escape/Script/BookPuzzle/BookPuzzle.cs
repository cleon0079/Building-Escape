using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BookPuzzle : MonoBehaviour
{
    UIManager uiManager;
    [SerializeField] string showedText = "Press F to interat";
    [SerializeField] GameObject target;
    [SerializeField] Camera puzzleCamera;

    private GameInput input;
    private InputAction interatAction;
    private InputAction escAction;
    private InputAction mousePosition;

    [SerializeField] LayerMask bookLayer;

    [SerializeField] List<GameObject> levelObjects;

    bool isDragging = false;
    float startMousePosition;

    Transform book;

    Animator animator;
    bool isDone = false;

    private void Awake()
    {
        input = new GameInput();
        interatAction = input.Player.Interart;
        escAction = input.Player.Esc;
        mousePosition = input.Player.MousePosition;

        interatAction.started += OnStart;
    }

    void OnStart(InputAction.CallbackContext context) {
        uiManager.StartPuzzle();

        uiManager.mainCamera.gameObject.SetActive(false);
        puzzleCamera.gameObject.SetActive(true);

        uiManager.ShowDot(false);
        uiManager.UpdateText("");

        uiManager.CursorMode(true);
        interatAction.started -= OnStart;
        interatAction.started += OnClick;
        interatAction.canceled += OnNoClick;

        mousePosition.Enable();
        escAction.Enable();
    }

    void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = puzzleCamera.ScreenPointToRay(mousePosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10, bookLayer))
        {
            isDragging = true;
            book = hit.transform;
            startMousePosition = mousePosition.ReadValue<Vector2>().x;
        }
    }

    void OnNoClick(InputAction.CallbackContext context) 
    {
        isDragging = false;
        CheckPuzzle();
    }

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        animator = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (isDragging)
        {
            float currentMousePosition = mousePosition.ReadValue<Vector2>().x;
            float deltaMousePosition = currentMousePosition - startMousePosition;

            float moveAmount = deltaMousePosition * Time.deltaTime * .5f; ;

            if (book.localPosition.x <= -1.5f && moveAmount < 0)
            {
                moveAmount = 0;
            }
            else if (book.localPosition.x >= 1.5f && moveAmount > 0)
            {
                moveAmount = 0;
            }
            book.localPosition += new Vector3(moveAmount,0,0);

            startMousePosition = currentMousePosition;
        }
    }

    void CheckPuzzle()
    {
        int i = 0;
        for (int k = 0; k < levelObjects.Count; k++)
        {
            if (levelObjects[k].transform.GetChild(i).localPosition.x < levelObjects[k].transform.GetChild(i + 1).localPosition.x)
            {
                if (levelObjects[k].transform.GetChild(i + 1).localPosition.x < levelObjects[k].transform.GetChild(i + 2).localPosition.x)
                {
                    PuzzleCompleted();
                }
            }
        }
    }

    private void PuzzleCompleted()
    {
        isDone = true;
        isDragging = false;

        uiManager.EndPuzzle();

        puzzleCamera.gameObject.SetActive(false);
        uiManager.mainCamera.gameObject.SetActive(true);

        uiManager.ShowDot(true);
        uiManager.CursorMode(false);

        animator.SetBool("BookShelfOpen", true);
        uiManager.UpdateText("");

        interatAction.Disable();
        escAction.Disable();
        mousePosition.Disable();
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isDone)
        {
            uiManager.UpdateText(showedText);
            interatAction.Enable();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isDone)
        {
            uiManager.UpdateText("");
            interatAction.Disable();
        }
    }
}
