using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // Reference to the player's camera
    [SerializeField] private GameObject targetObject;
    public float interactionDistance = 3f; // Distance for interaction
    [SerializeField] private KeyCode interactKey = KeyCode.F; // Key to interact

    void Update()
    {
    // Check if the interact key is pressed
    Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    RaycastHit hit;

        if (Physics.Raycast(ray, out hit)){
            // Check if the hit object is the target object
            if (hit.collider.gameObject == targetObject){
                // Check if the player presses the 'F' key
                if (Input.GetKeyDown(KeyCode.F)){
                     // Do something when 'F' is pressed while looking at the target object
                     Debug.Log("Player interacted with the target object.");
                }
            }
        }
    }
}
