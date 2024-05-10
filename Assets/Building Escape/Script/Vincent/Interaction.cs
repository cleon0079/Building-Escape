using System.Collections;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // Reference to the player's camera
    public float interactionDistance = 5f; // Distance for interaction
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Key to interact

    private bool isInteracting = false;
    private bool checkCrate =false;


    private GameObject currentCrate = null;
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
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Crate"))
            {
                Debug.Log("Looking at crate");
                currentCrate = hitObject;

                if (Input.GetKeyDown(interactKey))
                {
                    Debug.Log("Player look target object.");
                    // Start the interaction coroutine
                    StartCoroutine(InteractWithCrate(currentCrate.GetComponent<Crate>()));
                }
            }
        }
        else
        {
            currentCrate = null;
            Debug.Log("Not looking at crate");
            // Draw a debug line to visualize the raycast's direction when it doesn't hit anything
            Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);
        }
    }

    IEnumerator InteractWithCrate(Crate crate)
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
