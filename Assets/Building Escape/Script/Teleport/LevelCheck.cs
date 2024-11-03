using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class LevelCheck : MonoBehaviour
{
    Manager uiManager;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject finalDoor;
    

    private bool lv3open =false;
    private bool Finalopen =false;
    // Start is called before the first frame update
    void Start()
    {
        door.gameObject.SetActive(false);

        finalDoor.GetComponent<MeshRenderer>().enabled=false;
        finalDoor.gameObject.GetComponent<BoxCollider>().isTrigger=false;
        uiManager = FindObjectOfType<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(uiManager.level2Completed && lv3open == false)
        {
            door.gameObject.SetActive(true);
            lv3open = true;
        }

        if(uiManager.level3Completed && Finalopen == false)
        {
            finalDoor.GetComponent<MeshRenderer>().enabled = true;
            finalDoor.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            Finalopen = true;
        }
    }
}
