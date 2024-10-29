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
    private bool isOnInventory = false;
    public float rayDistance = 10f;
    public LayerMask collectableLayer;
    private string showedText = "Press F to collect";
    private DegreePuzzle degreePuzzle;

    GameInput input;
    InputAction inventoryAction;
    InputAction interartAction;
    InputAction escAction;
    Manager uIManager;
    UIManager uIManager2;
    private bool canPick = false;
    private void Awake()
    {
        input = new GameInput();
        inventoryAction = input.Player.Inventory;
        interartAction = input.Player.Interart;
        escAction = input.Player.Esc;

        inventoryAction.started += OnInventoryOpen;

        interartAction.started += PressToPick;
        interartAction.canceled += Release;

        escAction.started += OnCloseInventory;
    }

    private void Start()
    {
        uIManager = FindObjectOfType<Manager>();
        uIManager2 = FindObjectOfType<UIManager>();
        degreePuzzle = FindObjectOfType<DegreePuzzle>();
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
        uIManager2.EnableEscKey(true);
        ActiveMode(false);
        
    }

    void Update(){
        checkItem();
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
    void PressToPick(InputAction.CallbackContext callbackContext)
    {
        canPick = true;
    }
    void Release(InputAction.CallbackContext callbackContext)
    {
        canPick = false;
    }
    public void checkItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);
            if (Physics.Raycast(ray, out hit, rayDistance, collectableLayer))
            { 
                 
                
                if (hit.transform.CompareTag("BrokenDegree"))
                {
                    
                    if(degreePuzzle.canInterat){
                        uIManager.UpdateText(showedText);
                        interartAction.Enable();
                        ItemObject itemObject = hit.transform.GetComponent<ItemObject>();
                        if (itemObject != null)
                        {   
                            if(canPick)
                            {
                                AddItem(itemObject.item);
                                itemObject.PickUp(); 
                                return;
                            } 
                        }
                    }
                    else
                    {
                        interartAction.Disable();
                    }
                    
                }else if (hit.transform.CompareTag("PickableItem"))
                {
                    
                    if(hit.collider != null){
                        uIManager.UpdateText(showedText);
                        interartAction.Enable();
                        ItemObject itemObject = hit.transform.GetComponent<ItemObject>();
                        if (itemObject != null)
                        {   
                            if(canPick)
                            {
                                AddItem(itemObject.item);
                                itemObject.PickUp(); 
                                uIManager.UpdateText("");
                                
                                return;
                            } 
                        }
                    }
                    else
                    {
                        interartAction.Disable();
                    }
                    
                }
            }else{
                if(degreePuzzle.isPlayerExit)
                {
                    uIManager.UpdateText("");
                }
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
            DisplayItemsCanvas();
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
            if(item.Type == Item.Index.BookPuzzlePrize)
            {
                uIManager.bookShelfFinish = true;
            }
            if(item.Type == Item.Index.TablePuzzlePrize){
                uIManager.tablesFinish = true;
            }
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
        uIManager2.EnableEscKey(!active);
        uIManager.CursorMode(active);
    }
}