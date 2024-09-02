using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePrize : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 90;
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
