using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PaperPuzzlePlayerIn : MonoBehaviour
{
    bool isPuzzling = false;
    bool isDragging = false;
    int finishIndex = 0;

    [SerializeField] string showedText = "Press F to interat";
    [SerializeField] GameObject target;
    [SerializeField] GameObject puzzleTarget;

    private GameInput input;
    private InputAction interatAction;
    private InputAction escAction;
    private InputAction mousePosition;

    Manager uiManager;

    [SerializeField] LayerMask puzzleLayer;

    Vector3 startMousePosition;

    Transform puzzle;

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
        if (Physics.Raycast(ray, out hit, 2000, puzzleLayer))
        {
            if (hit.transform.GetComponent<PaperPuzzleItem>().GetPuzzleIn())
            {
                isDragging = false;
                hit.transform.parent.parent.GetComponent<PaperPuzzleFrame>().removePuzzle(hit.transform.GetComponent<PaperPuzzleItem>());
                hit.transform.SetParent(puzzleTarget.transform);
                hit.transform.position = puzzleTarget.transform.position;

                hit.transform.GetComponent<PaperPuzzleItem>().SetPuzzleIn(false);
                hit.transform.GetComponent<MeshCollider>().convex = true;
                hit.transform.gameObject.AddComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
            else
            {
                isDragging = true;
                puzzle = hit.transform;
                startMousePosition = new Vector3(mousePosition.ReadValue<Vector2>().x, mousePosition.ReadValue<Vector2>().y, uiManager.mainCamera.WorldToScreenPoint(puzzle.position).z);
            }
        }
    }

    void OnNoClick(InputAction.CallbackContext context)
    {
        isDragging = false;
    }

    void OnEsc(InputAction.CallbackContext context)
    {
        EscPuzzle();
    }

    private void Start()
    {
        uiManager = FindObjectOfType<Manager>();
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 currentMousePosition = new Vector3(mousePosition.ReadValue<Vector2>().x, mousePosition.ReadValue<Vector2>().y, uiManager.mainCamera.WorldToScreenPoint(puzzle.position).z);

            float deltaMousePositionX = currentMousePosition.x - startMousePosition.x;
            float deltaMousePositionY = currentMousePosition.y - startMousePosition.y;

            float moveAmountX = deltaMousePositionX * Time.deltaTime * .5f;
            float moveAmountY = deltaMousePositionY * Time.deltaTime * .5f;

            puzzle.localPosition += new Vector3(moveAmountY, 0, -moveAmountX);

            startMousePosition = currentMousePosition;
        }
    }

    public void Done() {
        finishIndex++;
        if (finishIndex == 3)
        {
            // TODO
            Debug.Log("Done");

            isDragging = false;

            uiManager.EndPuzzle();

            uiManager.mainCamera.transform.SetParent(FindObjectOfType<Controller>().transform);
            uiManager.mainCamera.transform.DOLocalMove(new Vector3(0, 1, 0), 1f);
            uiManager.mainCamera.transform.DOLocalRotate(Vector3.zero, 1f);

            uiManager.ShowDot(true);
            uiManager.CursorMode(false);

            escAction.started -= OnEsc;
            interatAction.started -= OnClick;
            interatAction.canceled -= OnNoClick;
            interatAction.Disable();
            escAction.Disable();
            mousePosition.Disable();
        }
    }

    void EscPuzzle()
    {
        isDragging = false;

        uiManager.EndPuzzle();

        uiManager.mainCamera.transform.SetParent(FindObjectOfType<Controller>().transform);
        uiManager.mainCamera.transform.DOLocalMove(new Vector3(0, 1, 0), 1f);
        uiManager.mainCamera.transform.DOLocalRotate(Vector3.zero, 1f);

        uiManager.ShowDot(true);
        uiManager.CursorMode(false);

        escAction.started -= OnEsc;
        interatAction.started -= OnClick;
        interatAction.canceled -= OnNoClick;

        interatAction.started += OnStart;
        uiManager.UpdateText(showedText);

        interatAction.Disable();
        escAction.Disable();
        mousePosition.Disable();

        isPuzzling = false;
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
