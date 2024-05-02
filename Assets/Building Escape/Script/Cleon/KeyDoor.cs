using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class KeyDoor : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoorOpen() {
        animator.SetBool("Door_Open", true);
        animator.SetBool("Door_Close", false);
        animator.SetBool("DoorStayClose", false);
        Debug.Log(13);

    }

    public void DoorClose() {
        animator.SetBool("Door_Open", false);
        animator.SetBool("Door_Close", true);
        animator.SetBool("DoorStayClose", false);
    }

    public void DoorStayClose() {
        animator.SetBool("Door_Open", false);
        animator.SetBool("Door_Close", false);
        animator.SetBool("DoorStayClose", true);
    }
}
