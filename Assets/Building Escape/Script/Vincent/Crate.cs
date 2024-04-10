using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Crate : MonoBehaviour
{
    private Animator crateAnimator;

    // Start is called before the first frame update
    void Start()
    {
        crateAnimator = GetComponent<Animator>();
    }

    // Method to open the crate
    public void Open()
    {
        crateAnimator.SetBool("Crate open", true);
    }

    // Method to close the crate
    public void Close()
    {
        crateAnimator.SetBool("Crate open", false);
    }

    // Check if the crate animation has reached a specific state (e.g., fully open)
    public bool IsAnimationComplete()
    {
        // Check if the crate animator exists
        if (crateAnimator == null)
        {
            Debug.LogWarning("Crate animator is not set.");
            return false;
        }

        // Check if the crate animation has reached a specific state
        return crateAnimator.GetCurrentAnimatorStateInfo(0).IsName("Crate_Open") &&
               !crateAnimator.IsInTransition(0);
    }
}
