using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // Reference to the player's camera
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Crate crate;
    public float interactionDistance = 3f; // Distance for interaction
    public KeyCode interactKey = KeyCode.F; // Key to interact

    void Update()
    {
    // Check if the interact key is pressed
    Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    RaycastHit hit;
         if (Physics.Raycast(ray, out hit, interactionDistance))
        {
        //if (Physics.Raycast(ray, out hit)){
            // Check if the hit object is the target object
            if (hit.collider.gameObject == targetObject){
                Debug.Log("Player look target object.");
                // Check if the player presses the 'F' key
                if (Input.GetKeyDown(KeyCode.F)){
                    // Do something when 'F' is pressed while looking at the target object
                    crate.Open();
                    
                    Debug.Log("Player interacted with the target object.");
                }
            }
        }
          else
        {
            // Draw a debug line to visualize the raycast's direction when it doesn't hit anything
            Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);
        }
    }
}
