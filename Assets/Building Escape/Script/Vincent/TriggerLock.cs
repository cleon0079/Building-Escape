using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLock : MonoBehaviour
{   
    [SerializeField] private PlayerController controller;
    [SerializeField] private Camera PlayerCamera;

    [SerializeField] private KeyCode interactKey = KeyCode.E; // Key to interact

    [SerializeField] private KeyCode EscapeKey = KeyCode.Escape; // Key to interact

    [SerializeField] private Camera SlideLockCamera;
    [SerializeField] private Camera numLockCamera;


    private bool IsTrigger;
    private bool IsSlideLock =false;
    private bool isNumLock = false;
    private GameManager gm;

    
    // Start is called before the first frame update
    void Start()
    {
        //Hide all other camera
        SlideLockCamera.enabled = false;
        numLockCamera.enabled = false;
    }

    // Update is called once per frame
    public void Update()    {
        if(IsTrigger ==true && Input.GetKeyDown(interactKey) ){
            //swicth off the player camera
            PlayerCamera.enabled =false;

            //Show the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            
            if(IsSlideLock ==true){
                NumSlidePuzzle();
            }

            if(isNumLock == true){
                NumLockPuzzle();
            }
        }

        //Excape the puzzle
        if (Input.GetKeyDown(EscapeKey)){
            //hide the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


            //switch back to player camera 
            PlayerCamera.enabled = true;

            //hide all other camera
            SlideLockCamera.enabled = false;

            numLockCamera.enabled = false;
            gm.numLock.CloseNumLock();
        }
    }

    void NumSlidePuzzle(){
        SlideLockCamera.enabled = true;
    }

    void NumLockPuzzle(){
        numLockCamera.enabled = true;
        gm.numLock.OpenNumLock();
    }


    void OnTriggerEnter(Collider  other){
        IsTrigger = true;

        if (other.CompareTag("SlideLock")){
            Debug.Log("Player entered the trigger!");
            IsSlideLock =true;
        }

        if (other.CompareTag("NumLock")){
             isNumLock = true;
        }
    }

    public void OnTriggerExit(Collider  other){
        IsTrigger = false;
        if (other.CompareTag("SlideLock")){
            Debug.Log("Player exited " + other);
            IsSlideLock =false;
        }

        if (other.CompareTag("NumLock"))
        {
            isNumLock = false;
        }
    }
}
