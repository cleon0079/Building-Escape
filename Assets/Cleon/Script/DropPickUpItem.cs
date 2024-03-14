using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPickUpItem : MonoBehaviour
{
    [SerializeField] Inventories inventory;
    [SerializeField] Transform dropPoint;
    [SerializeField] Camera camera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = camera.ViewportPointToRay(new Vector3(.5f, .5f, 0));
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 50f))
            {
                DropItem dropedItem = hitInfo.collider.gameObject.GetComponent<DropItem>();
                if (dropedItem != null)
                {
                    inventory.AddItem(dropedItem.item);
                    Destroy(hitInfo.collider.gameObject);
                }
            }

        }
    }

    public void DropItem()
    {
        if (inventory.selectedItem == null)
        {
            return;
        }

        GameObject mesh = inventory.selectedItem.Mesh;
        if (mesh != null)
        {
            GameObject spawnedMesh = Instantiate(mesh, null);

            spawnedMesh.transform.position = dropPoint.position;

            DropItem dropedItem = mesh.GetComponent<DropItem>();

            if (dropedItem != null)
            {
                dropedItem.item = new Item(inventory.selectedItem, 1);
            }
        }

        inventory.selectedItem.Amount--;
        if (inventory.selectedItem.Amount <= 0)
        {
            inventory.RemoveItem(inventory.selectedItem);
            inventory.selectedItem = null;
        }
    }
}