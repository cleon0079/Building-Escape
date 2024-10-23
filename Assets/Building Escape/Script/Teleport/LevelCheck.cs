using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class LevelCheck : MonoBehaviour
{
    Manager uiManager;
    [SerializeField] private GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        door.gameObject.SetActive(false); 
        
        uiManager = FindObjectOfType<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(uiManager.level2Completed)
        {
             door.gameObject.SetActive(true); 
        }
    }
}
