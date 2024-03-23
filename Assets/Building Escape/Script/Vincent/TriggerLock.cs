using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLock : MonoBehaviour
{   
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera LockCamera;

    private bool IsTrigger =false;
    // Start is called before the first frame update
    void Start()
    {
        LockCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTrigger ==true  && Input.GetKeyDown(KeyCode.E)){
            // Disable the main camera
            mainCamera.enabled = false;
            // Enable the Lock camera
            LockCamera.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape)){
             mainCamera.enabled = true;
            // Enable the Lock camera
            LockCamera.enabled = false;
        }
    }

     void OnTriggerEnter(Collider  other){
        if (other.CompareTag("Lock")){
            Debug.Log("Player entered the trigger!");
            IsTrigger =true;
        }
    }
}
