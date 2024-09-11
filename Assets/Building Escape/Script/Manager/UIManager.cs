using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    // Singleton
    static private UIManager instance;
    static public UIManager Instance 
    {
        get 
        {
            if (instance == null) 
            {
                Debug.LogError("There is not UIManager in the scene.");
            }            
            return instance;
        }
    }
    [SerializeField] private Manager manager;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private Button startButton;
    void Awake() 
    {
        if (instance != null)
        {
            // there is already a UIManager in the scene, destory this one
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
      
    }

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(SetStartpanelOff);
        manager.CursorMode(true);
        manager.StartPuzzle();
        startPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetStartpanelOff()
    {
        startPanel.SetActive(false);
        manager.CursorMode(false);
        manager.EndPuzzle();
        Debug.Log("sdad");
    }
}
