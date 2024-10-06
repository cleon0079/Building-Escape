using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoPuzzle : MonoBehaviour
{
    private Collider[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        colliders = new Collider[this.transform.childCount];

        for (int i = 0; i < this.transform.childCount; i++)
        {
            colliders[i] = this.transform.GetChild(i).GetComponent<Collider>();
        }

        for (int i = 0; i < this.transform.childCount; i++)
        {
            for (int j = i + 1; j < this.transform.childCount; j++)
            {
                Physics.IgnoreCollision(colliders[i], colliders[j]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
