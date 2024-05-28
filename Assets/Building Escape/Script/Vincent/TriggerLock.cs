using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLock : MonoBehaviour
{   
    [SerializeField] private PlayerController controller;
    [SerializeField] private Camera PlayerCamera;

    [SerializeField] GameManager gm;

    [SerializeField] private Camera SlideLockCamera;

    [SerializeField] private SlidePuzzle slidePuzzle;

    private bool IsTriggerLock;
    private bool IsSlideLock =false;
    private bool puzzleCompletedHandled = false;
    
    // Start is called before the first frame update
    void Start()
    {   
        //Hide all other camera
        SlideLockCamera.enabled = false;
        //numLockCamera.enabled = false;

        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    public void Update()    {
        if (IsTriggerLock == true)
        {
            if (Input.GetKeyDown(gm.getInteractKey()) && IsSlideLock == true)
            {
                //swicth off the player camera
                //PlayerCamera.enabled = false;

                //Show the cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                gm.noInteract();

                if (IsSlideLock == true)
                {
                    NumSlidePuzzle();
                }
            }

            //Excape the puzzle
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //hide the cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                gm.yesInteract();

                //switch back to player camera 
                PlayerCamera.enabled = true;

                //hide all other camera
                SlideLockCamera.enabled = false;
            }
        }

        if (slidePuzzle.puzzleComplete == true && !puzzleCompletedHandled ) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            gm.yesInteract();

            //switch back to player camera 
            PlayerCamera.enabled = true;

            //hide all other camera
            SlideLockCamera.enabled = false;

            //disable this statment
            puzzleCompletedHandled = true;
        }
    }

    void NumSlidePuzzle(){
        SlideLockCamera.enabled = true;
    }

    void OnTriggerEnter(Collider  other){
        

        if (other.CompareTag("SlideLock")){
            IsTriggerLock = true;
            IsSlideLock =true;
        }

    }

    public void OnTriggerExit(Collider  other){
        
        if (other.CompareTag("SlideLock")){
            IsTriggerLock = false;
            IsSlideLock =false;
        }
    }
    
}
