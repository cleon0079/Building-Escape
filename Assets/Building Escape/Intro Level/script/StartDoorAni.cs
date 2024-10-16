using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDoorAni : MonoBehaviour
{
    private Animator animator;
    private bool playerEnter;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerEnter = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerEnter){
            animator.SetBool("DoorOpen" , true);
        }
        if(!playerEnter){
            animator.SetBool("DoorOpen" , false);
        }
        if(playerEnter){
            animator.SetBool("Enter" , true);
        }
        if(!playerEnter){
            animator.SetBool("Enter" , false);
        }
    }

    void OnTriggerEnter (Collider collider){
        if(collider.gameObject.layer ==6 ){
            playerEnter = true;
        }
    }

    void OnTriggerExit(Collider collider){
        if(collider.gameObject.layer ==6 ){
            playerEnter = false;
        }
    }
}
