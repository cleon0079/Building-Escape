using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLock : MonoBehaviour
{   
    
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera LockCamera;

    private bool IsTrigger =false;
    // Start is called before the first frame update
    void Start()
    {
        LockCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTrigger ==true){
            // Disable the main camera
            mainCamera.enabled = false;
            // Enable the Lock camera
            LockCamera.enabled = true;
        }
    }

     void OnTriggerEnter(Collider  other){
        if (other.CompareTag("Lock")){
            Debug.Log("Player entered the trigger!");
            IsTrigger =true;
        }
    }
}
