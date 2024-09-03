using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BookPuzzle : MonoBehaviour
{
    GameManager uiManager;
    [SerializeField] string showedText = "Press F to interat";
    [SerializeField] GameObject target;

    private GameInput input;
    private InputAction interatAction;
    private InputAction escAction;
    private InputAction mousePosition;

    [SerializeField] LayerMask bookLayer;

    [SerializeField] List<GameObject> levelObjects;

    bool isPuzzling = false;

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

        uiManager.mainCamera.transform.SetParent(target.transform);
        uiManager.mainCamera.transform.DOLocalMove(Vector3.zero, 1f);
        uiManager.mainCamera.transform.DOLocalRotate(Vector3.zero, 1f);

        uiManager.ShowDot(false);
        uiManager.UpdateText("");

        uiManager.CursorMode(true);
        interatAction.started -= OnStart;
        interatAction.started += OnClick;
        interatAction.canceled += OnNoClick;
        escAction.started += OnEsc;

        mousePosition.Enable();
        escAction.Enable();

        isPuzzling = true;
    }

    void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = uiManager.mainCamera.ScreenPointToRay(mousePosition.ReadValue<Vector2>());
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

    void OnEsc(InputAction.CallbackContext context) 
    {
        EscPuzzle();
    }

    private void Start()
    {
        uiManager = FindObjectOfType<GameManager>();
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
                    if (k == levelObjects.Count - 1)
                    {
                        PuzzleCompleted();
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void EscPuzzle() 
    {
        isDragging = false;

        uiManager.EndPuzzle();

        uiManager.mainCamera.transform.SetParent(FindObjectOfType<Controller>().transform);
        uiManager.mainCamera.transform.DOLocalMove(Vector3.zero, 1f);
        uiManager.mainCamera.transform.DOLocalRotate(Vector3.zero, 1f);

        uiManager.ShowDot(true);
        uiManager.CursorMode(false);

        interatAction.started -= OnClick;
        interatAction.canceled -= OnNoClick;
        escAction.started -= OnEsc;

        interatAction.started += OnStart;
        uiManager.UpdateText(showedText);

        interatAction.Disable();
        escAction.Disable();
        mousePosition.Disable();

        isPuzzling = false;
    }

    private void PuzzleCompleted()
    {
        isDone = true;

        EscPuzzle();
        interatAction.started -= OnStart;

        animator.SetBool("BookShelfOpen", true);
        uiManager.UpdateText("");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isDone)
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isDone)
        {
            uiManager.UpdateText("");
            interatAction.Disable();
        }
    }
}
