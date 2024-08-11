using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Inventories : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] List<Item> Inventory = new List<Item>();
    [NonSerialized] public Item selectedItem = null;

    [Header("Inventory UI Element")]
    [SerializeField] Button buttonPrefab;
    [SerializeField] GameObject inventoryGameObject;
    [SerializeField] GameObject inventoryContent;
    

    private Camera camera;
    private GameManager gm;

    [Header("ZoomIn UI Element")]
    [SerializeField] GameObject zoomInGameObject;
    [SerializeField] Button dropButton;
    [SerializeField] Button closeButton;

    private void Start()
    {
        camera = Camera.main;
        gm = FindAnyObjectByType<GameManager>();
    }

    public void AddItem(Item _item)
    {
        if (Inventory.Count >= 8)
        {
            return;
        }
        else
        {
            Inventory.Add(_item);
        }
    }

    public void RemoveItem(Item _item)
    {
        if (Inventory.Contains(_item))
        {
            Inventory.Remove(_item);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && gm.GetLock() == false)
        {

            if (inventoryGameObject.activeSelf)
            {
                gm.CursorLock(true);
                inventoryGameObject.SetActive(false);
                gm.yesInteract();
            }
            else
            {
                gm.CursorLock(false);
                inventoryGameObject.SetActive(true);
                DisplayItemsCanvas();
                gm.noInteract();
            }
        }
    }

    void DestroyAllChildren(Transform _parent)
    {
        foreach (Transform child in _parent)
        {
            Destroy(child.gameObject);
        }
    }

    void DisplayItemsCanvas()
    {
        DestroyAllChildren(inventoryContent.transform);
        for (int i = 0; i < Inventory.Count; i++)
        {
            Button buttonGO = Instantiate<Button>(buttonPrefab, inventoryContent.transform);
            buttonGO.name = Inventory[i].Name + " Button";
            RawImage itemImage = buttonGO.GetComponent<RawImage>();
            
            Item item = Inventory[i];
            itemImage.texture = item.Icon;

            buttonGO.onClick.AddListener(() => {  DisplaySelectedItemOnCanvas(item); });
            
        }
    }


    public void DisplaySelectedItemOnCanvas(Item _item)
    {
        selectedItem = _item;
        inventoryGameObject.SetActive(false);
        zoomInGameObject.SetActive(true);
        camera.orthographic = true;
        camera.orthographicSize = 1.5f;
        GameObject mesh = selectedItem.Mesh;
        if (mesh != null)
        {
            mesh.GetComponentInChildren<MeshRenderer>().enabled = true;
            mesh.transform.parent = camera.transform;
            mesh.transform.localPosition = new Vector3(0, 0, 1);
            mesh.GetComponent<DropItem>().SetView(true);
            gm.IsLock(true);
        }
        closeButton.onClick.AddListener(() => { CloseSelectedItemCanvas(); });
        dropButton.onClick.AddListener(() => { DropSelectedItem(); });
        
    }

    void CloseSelectedItemCanvas() {
        camera.orthographic = false;
        camera.fieldOfView = 60f;
        zoomInGameObject.SetActive(false);
        gm.IsLock(false);
        gm.CursorLock(true);
        for (int i = 0; i < camera.transform.childCount; i++)
        {
            GameObject mesh = camera.transform.GetChild(i).gameObject;
            mesh.GetComponent<BoxCollider>().enabled = false;
            mesh.GetComponentInChildren<MeshRenderer>().enabled = false;
            mesh.GetComponent<DropItem>().SetView(false);
        }
        gm.yesInteract();
    }

    void DropSelectedItem() {
        CloseSelectedItemCanvas();
        gm.dropPickUpItem.DropItem();
        gm.yesInteract();
    }

    public List<Item> getInventory() {
        return Inventory;
    }
}