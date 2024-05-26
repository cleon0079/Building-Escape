using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    Inventories Inventories;
    bool key = false;
    [SerializeField] GameManager gm;

    private void Start()
    {
        Inventories = FindObjectOfType<Inventories>();
        gm = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(gm.getInteractKey()))
        {
            CheckKey();
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 50f) && key)
            {

                if (hitInfo.collider.gameObject.CompareTag("Door"))
                {
                    hitInfo.collider.gameObject.GetComponent<Animator>().SetBool("Open", true);
                }

            }
        }
    }

    void CheckKey() {
        List<Item> inventory = Inventories.getInventory();
        foreach (Item item in inventory)
        {
            if (item.Type == Item.Index.KeyDoor)
            {
                key = true;
            }
        }
    }
}
