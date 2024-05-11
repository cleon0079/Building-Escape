using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    Inventories Inventories;
    bool key = false;

    private void Start()
    {
        Inventories = FindObjectOfType<Inventories>();
    }

    // Update is called once per frame
    void Update()
    {
        List<Item> inventory = Inventories.getInventory();
        foreach (Item item in inventory)
        {
            if (item.Type == Item.Index.KeyDoor)
            {
                key = true;
            }
        }


        if (Input.GetKeyDown(KeyCode.F) && key)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 50f))
            {

                if (hitInfo.collider.gameObject.CompareTag("Door"))
                {
                    hitInfo.collider.gameObject.GetComponent<Animator>().SetBool("Open", true);
                }

            }
        }
    }
}
