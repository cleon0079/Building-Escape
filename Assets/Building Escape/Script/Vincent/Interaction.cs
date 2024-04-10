using System.Collections;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // Reference to the player's camera
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Crate crate;
    public float interactionDistance = 5f; // Distance for interaction
    [SerializeField] private KeyCode interactKey = KeyCode.F; // Key to interact

    private bool isInteracting = false;
    private bool checkCrate =false;
    void Update()
    {
        // Check if already interacting or not
        if (isInteracting)
            return;

        // Check if the interact key is pressed
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {   
            if(hit.collider.gameObject == targetObject){
                Debug.Log("Look crate");
                checkCrate = true; 
            }

            
            if (Input.GetKeyDown(interactKey) && checkCrate ==true)
                {
            // Debug.Log("crate open");
            // Check if the hit object is the target object
            // if (hit.collider.gameObject == targetObject)
            // {
                Debug.Log("Player look target object.");
                // Check if the player presses the 'F' key
                   
                    
                    // Start the interaction coroutine
                    StartCoroutine(InteractWithCrate());
                // }
            }
        }
        else
        {   
            checkCrate = false;
            Debug.Log("crate false");
            // Draw a debug line to visualize the raycast's direction when it doesn't hit anything
            Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);
        }
    }

    IEnumerator InteractWithCrate()
    {
        // Set interacting flag to true
        isInteracting = true;

        // Open the crate
        crate.Open();

        // Wait until the crate animation is complete
        yield return new WaitUntil(() => crate.IsAnimationComplete());

        // Interaction complete
        Debug.Log("Player interacted with the target object.");

        // Reset interacting flag
        isInteracting = false;
    }
}
