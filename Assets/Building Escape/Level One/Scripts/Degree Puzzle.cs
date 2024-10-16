using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DegreePuzzle : MonoBehaviour
{
    private Animator puzzleAni;
    [SerializeField] string showedText = "Press F to interat";
    private GameInput input;
    private InputAction interatAction;
     private InputAction mousePosition;
     float startMousePosition;
    Manager uiManager;
    [SerializeField] LayerMask degreeLayer;
    Transform book;
    private UIManager uIManager2;
    bool isPuzzling = false;
    void Awake()
    {
        input = new GameInput();
        interatAction = input.Player.Interart;
    }
    void OnStart()
    {

    }
    void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = uiManager.mainCamera.ScreenPointToRay(mousePosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10, degreeLayer))
        {
            book = hit.transform;
            startMousePosition = mousePosition.ReadValue<Vector2>().x;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<Manager>();
        puzzleAni = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("yes");
            uIManager2.EnableEscKey(false);
            uiManager.UpdateText(showedText);
            interatAction.Enable();
        }
    }
}
