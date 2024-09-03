using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventories : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] List<Item> Inventory = new List<Item>();

    [Header("Inventory UI Element")]
    [SerializeField] Image prize;
    [SerializeField] GameObject inventoryGameObject;
    [SerializeField] GameObject inventoryContent;

    GameInput input;
    InputAction inventoryAction;
    InputAction escAction;

    Manager uIManager;

    private void Awake()
    {
        input = new GameInput();
        inventoryAction = input.Player.Inventory;
        escAction = input.Player.Esc;

        inventoryAction.started += OnInventoryOpen;
        escAction.started += OnCloseInventory;
    }

    private void Start()
    {
        uIManager = FindObjectOfType<Manager>();
    }

    private void OnEnable()
    {
        inventoryAction.Enable();
    }

    private void OnDisable()
    {
        inventoryAction.Disable();
    }

    void OnCloseInventory(InputAction.CallbackContext callbackContext) {
        ActiveMode(false);
    }

    void OnInventoryOpen(InputAction.CallbackContext callbackContext) {
        if (inventoryGameObject.activeSelf)
        {
            ActiveMode(false);
        }
        else
        {
            ActiveMode(true);
        }
    }

    public void AddItem(Item _item)
    {
        if (Inventory.Count >= 10)
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
            Image imageGO = Instantiate<Image>(prize, inventoryContent.transform);
            Item item = Inventory[i];
            imageGO.sprite = item.Image;    
        }
    }

    public void CanInventory(bool canOpen) {
        if (canOpen)
        {
            inventoryAction.Enable();
        }
        else
        {
            inventoryAction.Disable();
        }
    }

    public void CloseInventory() {
        ActiveMode(false);
    }

    public void ActiveMode(bool active) {
        inventoryGameObject.SetActive(active);
        if (active)
        {
            escAction.Enable();
        }
        else
        {
            escAction.Disable();
        }
        uIManager.Inventory(!active);
        uIManager.CursorMode(active);
    }
}