using System.Collections;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // Reference to the player's camera
    public float interactionDistance = 5f; // Distance for interaction
    [SerializeField] private KeyCode interactKey = KeyCode.F; // Key to interact

    private bool isInteracting = false;

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
                //get target crate that looking
                currentCrate = hitObject;

                if (Input.GetKeyDown(interactKey))
                {
                    OpenCrate(currentCrate.GetComponent<Crate>());
                }
            }
        }
        else
        {
            currentCrate = null;
            // Draw a debug line to visualize the raycast's direction when it doesn't hit anything
            Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);
        }
    }

    void OpenCrate(Crate crate)
    {
        isInteracting = true;

        crate.Open();

        isInteracting = false;
    }
}
