using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenButton : MonoBehaviour
{
    [SerializeField] Animator am;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 50f))
            {

                if (hitInfo.collider.gameObject.CompareTag("Door"))
                {
                    am.SetBool("Enter", true);
                }

            }
        }
    }
}
