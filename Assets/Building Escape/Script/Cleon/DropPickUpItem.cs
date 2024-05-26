using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPickUpItem : MonoBehaviour
{
    private Camera camera;
    private GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(gm.getInteractKey()) && gm.GetLock() == false)
        {
            Ray ray = camera.ViewportPointToRay(new Vector3(.5f, .5f, 0));
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 50f))
            {
                DropItem dropedItem = hitInfo.collider.gameObject.GetComponent<DropItem>();
                if (dropedItem != null)
                {
                    gm.inventories.AddItem(dropedItem.item);
                    hitInfo.transform.gameObject.GetComponent<BoxCollider>().enabled = false;

                    if (hitInfo.transform.childCount == 0)
                    {
                        hitInfo.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
                        
                    }
                    else
                    {
                        for (int i = 0; i < hitInfo.collider.transform.childCount; i++)
                        {
                            hitInfo.collider.transform.GetChild(i).gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                        }
                    }
                    dropedItem.GetItem();
                }
            }

        }
    }

    public void DropItem()
    {
        if (gm.inventories.selectedItem == null)
        {
            return;
        }

        GameObject mesh = gm.inventories.selectedItem.Mesh;
        if (mesh != null)
        {
            mesh.GetComponentInChildren<MeshRenderer>().enabled = true;
            mesh.GetComponentInChildren<BoxCollider>().enabled = true;
            mesh.transform.parent = null;
            mesh.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
            

            DropItem dropedItem = mesh.GetComponent<DropItem>();
            dropedItem.Drop();
            if (dropedItem != null)
            {
                dropedItem.item = new Item(gm.inventories.selectedItem);
            }
        }

        gm.inventories.RemoveItem(gm.inventories.selectedItem);
        gm.inventories.selectedItem = null;
    }
}