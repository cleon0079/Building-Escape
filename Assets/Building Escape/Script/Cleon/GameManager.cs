using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [NonSerialized] public bool isPause = false;
    private bool isLock = false;
    [SerializeField] private GameObject ui;

    [SerializeField] Animator am;

    [Header("Reference")]
    public PlayerController playerController;
    public Inventories inventories;
    public DropPickUpItem dropPickUpItem;

    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject endMenu;

    [SerializeField] KeyCode interact = KeyCode.F;
    //public KeyDoor keyDoor;
    //public NumLock numLock;

    // Start is called before the first frame update
    void Start()
    {
        startMenu.SetActive(true);
        CursorLock(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OpenKeyDoor();
            Debug.Log("111");
        }*/

        if (isLock && Input.GetKeyDown(interact))
        {
            ui.SetActive(true);
            CursorLock(false);
            noInteract();
            //am.SetBool("Open", true);
        }
        else if (ui.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ui.SetActive(false);
            CursorLock(true);
            yesInteract();
        }
    }

    public KeyCode getInteractKey() {
        return interact;
    }


    public void noInteract() {
        interact = KeyCode.None;
    }

    public void yesInteract() {
        interact = KeyCode.F;
    }

    public void OpenGlassDoor() {
        am.SetBool("Open", true);
        ui.SetActive(false);
        CursorLock(true);
    }

    public void IsLock(bool isClose) {
        this.isLock = isClose;
    }

    public bool GetLock() {
        return isLock;
    }

    public bool GetPause() {
        return isPause;
    }

    public void CursorLock(bool isLock) {
        if (isLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = !isLock;
            playerController.CanMove(isLock);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = !isLock;
            playerController.CanMove(isLock);
        }
    }

    public void StartGame() {
        startMenu.SetActive(false);

        CursorLock(true);
    }

    public void EndGame() {
        endMenu.SetActive(true);
        CursorLock(false);
    }

    public void ExitGame(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
