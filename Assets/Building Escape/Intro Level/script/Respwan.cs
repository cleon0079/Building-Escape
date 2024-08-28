using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Vector3 lastCheckPoint;
    public Vector3 LastCheckPoint
    {
        get
        {
            return lastCheckPoint;
        }
    }
    [SerializeField] private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("LastcheckP"+lastCheckPoint);
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer == 6)
        {
            lastCheckPoint = player.transform.position;
        }
    }
}
