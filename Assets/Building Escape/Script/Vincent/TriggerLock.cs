using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLock : MonoBehaviour
{   
    
    [SerializeField] private PlayerPrefs player;

    [SerializeField] private Camera PlayerCamera;

    [SerializeField] private Camera NumSlideLockCamera;
    [SerializeField] private Camera numLockCamera;



    private bool isNumLock = false;
    private GameManager gm;

    private bool IsTrigger =false;
    // Start is called before the first frame update
    void Start()
    {
        NumSlideLockCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void NumSlidePuzzle(){
        
    }

    void NumLockPuzzle(){

    }


    void OnTriggerEnter(Collider  other){
        if (other.CompareTag("Lock")){
            Debug.Log("Player entered the trigger!");
            IsTrigger =true;
        }

        if (other.CompareTag("NumLock")){
             isNumLock = true;
        }
    }

    public void OnTriggerExit(Collider  other){
        if (other.CompareTag("Lock")){
            Debug.Log("Player exited " + other);
            IsTrigger =false;
        }

        if (other.CompareTag("NumLock"))
        {
            isNumLock = false;
        }
    }
}
