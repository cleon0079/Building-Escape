using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{

    [SerializeField] private Camera playerCamera; // Reference to the player's camera
    public float interactionDistance = 5f; // Distance for interaction
    [SerializeField] private GameObject ToolHint; // Look the intercat obj

    [SerializeField] private GameObject PhotoFrameHint;

    [SerializeField] private Jigsaw jigsaw;

    // Start is called before the first frame update
    void Start()
    {
        ToolHint.SetActive(false);
        PhotoFrameHint.SetActive(false);

        jigsaw =FindFirstObjectByType<Jigsaw>();
    }


    // Update is called once per frame
    void Update()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Jigsaw")  || hitObject.CompareTag("Door") || hitObject.CompareTag("Crate") || hitObject.CompareTag("ID_Card") || hitObject.CompareTag("Clue"))
            {
              ToolHint.SetActive(true);
            }

            if (hitObject.CompareTag("PhotoFrame") && jigsaw.Complete ==false)
            {
                PhotoFrameHint.SetActive(true);
            }
        }

        else
        {
            ToolHint.SetActive(false);
            PhotoFrameHint.SetActive(false);

        }
    }

}
