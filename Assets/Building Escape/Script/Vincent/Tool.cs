using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{

    [SerializeField] private Camera playerCamera; // Reference to the player's camera
    public float interactionDistance = 5f; // Distance for interaction
    [SerializeField] private GameObject ToolHint; // Key to interact

    // Start is called before the first frame update
    void Start()
    {
        ToolHint.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Jigsaw") || hitObject.CompareTag("NumLock") || hitObject.CompareTag("Door") || hitObject.CompareTag("SlideLock"))
            {
                ToolHint.SetActive(true);
            }
        }
        else
        {
            ToolHint.SetActive(false);
        }
    }
}
