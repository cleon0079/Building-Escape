using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [NonSerialized] public bool isZoom = false;
    [NonSerialized] public bool isPause = false;

    [Header("Reference")]
    public FPSController fpsController;
    public Inventories inventories;
    public DropPickUpItem dropPickUpItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IsZoom(bool _isZoom) {
        isZoom = _isZoom;
    }

    public bool GetZoom() {
        return isZoom;
    }

    public bool GetPause() {
        return isPause;
    }

    public void CursorLock(bool isLock) {
        if (isLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = !isLock;
            fpsController.canMove = isLock;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = !isLock;
            fpsController.canMove = isLock;
        }
    }
}
