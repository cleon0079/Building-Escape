using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenButton : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(111);
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 50f))
            {
                Debug.Log(222);
                if (hitInfo.collider.gameObject.CompareTag("Door"))
                {
                    Debug.Log(333);
                    animator.SetBool("Enter", true);
                }
            }
        }
    }
}
