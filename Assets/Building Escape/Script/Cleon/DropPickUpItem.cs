using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPickUpItem : MonoBehaviour
{
    private Camera camera;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && gameManager.GetZoom() == false)
        {
            Ray ray = camera.ViewportPointToRay(new Vector3(.5f, .5f, 0));
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 50f))
            {
                DropItem dropedItem = hitInfo.collider.gameObject.GetComponent<DropItem>();
                if (dropedItem != null)
                {
                    gameManager.inventories.AddItem(dropedItem.item);
                    hitInfo.collider.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                    dropedItem.GetItem();
                }
            }

        }
    }

    public void DropItem()
    {
        if (gameManager.inventories.selectedItem == null)
        {
            return;
        }

        GameObject mesh = gameManager.inventories.selectedItem.Mesh;
        if (mesh != null)
        {
            mesh.GetComponentInChildren<MeshRenderer>().enabled = true;
            mesh.transform.parent = null;
            mesh.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
            

            DropItem dropedItem = mesh.GetComponent<DropItem>();
            dropedItem.Drop();
            if (dropedItem != null)
            {
                dropedItem.item = new Item(gameManager.inventories.selectedItem);
            }
        }

        gameManager.inventories.RemoveItem(gameManager.inventories.selectedItem);
        gameManager.inventories.selectedItem = null;
    }
}