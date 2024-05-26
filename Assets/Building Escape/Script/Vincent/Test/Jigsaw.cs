using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jigsaw : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // Reference to the player's camera
    [SerializeField] private float interactionDistance = 6f; // Distance for interaction
    [SerializeField] GameManager gm;


    private bool isInteracting = false;
    private int count;

    //set the piece of the origin
    [SerializeField] private GameObject PhotoFrame;
    [SerializeField] private GameObject[] PhotoFramePiece;

    //set tje piece that in the map
    [SerializeField] private GameObject jigsaw;
    [SerializeField] private GameObject[] jigsawPieces;

    private int PhotoFrameCount;
    private int jigsawCount;

    //check the complete the jigsaw
    private bool Complete =false;

    [SerializeField] private KeyDoor JigsawDoor;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();

        //set the parent object
        PhotoFrameCount = PhotoFrame.transform.childCount;
        jigsawCount = jigsaw.transform.childCount;

        //set  the childe object of jigsaw Pieces
        PhotoFramePiece = new GameObject[PhotoFrameCount];
        jigsawPieces =new GameObject[jigsawCount];

        for (int i  = 0;i < jigsawCount; i++)
        {
            jigsawPieces[i] = jigsaw.transform.GetChild(i).gameObject;
            jigsawPieces[i].AddComponent<BoxCollider>();

        }

        for (int i = 0; i < PhotoFrameCount; i++)
        {
            PhotoFramePiece[i] = PhotoFrame.transform.GetChild(i).gameObject;
            PhotoFramePiece[i].SetActive(false);
        }
        


    }

    // Update is called once per frame
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

            if (hitObject.CompareTag("Jigsaw") && Input.GetKeyDown(gm.getInteractKey()))
            {
                for (int i = 0; i < jigsawCount; i++)
                {
                    if(hitObject == jigsawPieces[i])
                    {
                        jigsawPieces[i].SetActive(false);
                        PhotoFramePiece[i].SetActive(true);
                        count ++;
                        break;
                    }
                }
            }
        }

        else
        {
            // Draw a debug line to visualize the raycast's direction when it doesn't hit anything
            Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.red);
        }

        checkComplete();
    }


    private void checkComplete()
    {
        if (count == PhotoFrameCount && Complete ==false)
        {
            Debug.Log(12);
            JigsawDoor.DoorOpen();
            Complete = true;
        }
    }
}
