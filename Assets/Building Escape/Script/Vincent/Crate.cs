using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Crate : MonoBehaviour
{
    Animator CrateAnimator;
    // Start is called before the first frame update
    void Start()
    {
        CrateAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
     public void Open(){
        CrateAnimator.SetBool("Crate open",true);
    }

    public void Close(){
        CrateAnimator.SetBool("Crate open",false);
    }
}
