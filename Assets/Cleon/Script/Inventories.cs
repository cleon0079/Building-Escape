using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Inventories : MonoBehaviour
{
    [SerializeField] List<Item> Inventory = new List<Item>();
    [SerializeField] bool showIMGUIInventory = true;
    [NonSerialized] public Item selectedItem = null;

    [SerializeField] Button buttonPrefab;
    [SerializeField] GameObject inventoryGameObject;
    [SerializeField] GameObject inventoryContent;
    [SerializeField] GameObject FilterContent;

    Vector2 scrollPosition;
    string sortType = "All";

    [Header("Selected Item Display")]
    [SerializeField] RawImage itemImage;
    [SerializeField] Text itemName;
    [SerializeField] Text itemDescription;

    private void Start()
    {
        DisplayFilterCanvas();
    }

    public void AddItem(Item _item)
    {
        AddItem(_item, _item.Amount);
    }

    public void AddItem(Item _item, int _count)
    {
        Item foundItem = Inventory.Find((x) => x.Name == _item.Name);

        if (foundItem == null)
        {
            Inventory.Add(_item);
        }
        else
        {
            foundItem.Amount += _count;
        }
        DisplayItemsCanvas();
        DisplaySelectedItemOnCanvas(selectedItem);
    }

    public void RemoveItem(Item _item)
    {
        if (Inventory.Contains(_item))
        {
            Inventory.Remove(_item);
        }

        DisplayItemsCanvas();
        DisplaySelectedItemOnCanvas(selectedItem);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {

            if (inventoryGameObject.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                inventoryGameObject.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                inventoryGameObject.SetActive(true);
                DisplayItemsCanvas();
            }
        }
    }

    void DisplayFilterCanvas()
    {
        List<string> itemTypes = new List<string>(Enum.GetNames(typeof(Item.ItemType)));
        itemTypes.Insert(0, "All");

        for (int i = 0; i < itemTypes.Count; i++)
        {
            Button buttonGO = Instantiate<Button>(buttonPrefab, FilterContent.transform);
            Text buttonText = buttonGO.GetComponentInChildren<Text>();
            buttonGO.name = itemTypes[i] + " Filter";
            buttonText.text = itemTypes[i];
            buttonText.fontSize = 20;

            string itemType = itemTypes[i];
            buttonGO.onClick.AddListener(() => { ChangeFilter(itemType); });
            //buttonGO.onClick.AddListener(delegate { ChangeFilter(itemTypes[x]); });
        }
    }

    void ChangeFilter(string _itemType)
    {
        sortType = _itemType;
        DisplayItemsCanvas();
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
            if (Inventory[i].Type.ToString() == sortType || sortType == "All")
            {
                Button buttonGO = Instantiate<Button>(buttonPrefab, inventoryContent.transform);
                Text buttonText = buttonGO.GetComponentInChildren<Text>();
                buttonGO.name = Inventory[i].Name + " Button";
                buttonText.text = Inventory[i].Name;
                buttonText.fontSize = 20;

                Item item = Inventory[i];
                buttonGO.onClick.AddListener(() => { DisplaySelectedItemOnCanvas(item); });
            }
        }
    }

    void DisplaySelectedItemOnCanvas(Item _item)
    {
        selectedItem = _item;

        itemImage.texture = selectedItem.Icon;
        itemName.text = selectedItem.Name;
        itemDescription.text = selectedItem.Description +
            "\nValue: " + selectedItem.Value +
            "\nAmount: " + selectedItem.Amount;
    }

    private void OnGUI()
    {
        if (showIMGUIInventory)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

            List<string> itemTypes = new List<string>(Enum.GetNames(typeof(Item.ItemType)));
            itemTypes.Insert(0, "All");

            for (int i = 0; i < itemTypes.Count; i++)
            {
                if (GUI.Button(new Rect(
                    (Screen.width / itemTypes.Count) * i,
                    10,
                    Screen.width / itemTypes.Count,
                    20), itemTypes[i]))
                {
                    sortType = itemTypes[i];
                }
            }

            Display();
            if (selectedItem != null)
            {
                DisplaySelecterItem();
            }
        }
    }

    void DisplaySelecterItem()
    {
        GUI.Box(new Rect(Screen.width / 4, Screen.height / 3,
            Screen.width / 5, Screen.height / 5),
            selectedItem.Icon);

        GUI.Box(new Rect(Screen.width / 4, (Screen.height / 3) + (Screen.height / 5),
            Screen.width / 7, Screen.height / 15),
            selectedItem.Name);

        GUI.Box(new Rect(Screen.width / 4, (Screen.height / 3) + (Screen.height / 3),
            Screen.width / 5, Screen.height / 5), selectedItem.Description +
            "\nValue: " + selectedItem.Value +
            "\nAmount: " + selectedItem.Amount);
    }

    void Display()
    {
        scrollPosition = GUI.BeginScrollView(new Rect(0, 40, Screen.width, Screen.height - 40),
            scrollPosition,
            new Rect(0, 0, 0, Inventory.Count * 30),
            false,
            true);
        int count = 0;
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].Type.ToString() == sortType || sortType == "All")
            {
                if (GUI.Button(new Rect(30, 0 + (count * 30), 200, 30), Inventory[i].Name))
                {
                    selectedItem = Inventory[i];
                    selectedItem.OnClicked();
                }
                count++;
            }
        }
        GUI.EndScrollView();
    }
}