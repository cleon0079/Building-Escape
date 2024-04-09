using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeySystem
{
    public class KeyDoorController : MonoBehaviour
    {
        private Animator doorAnim;
        private bool doorOpen = false;

        [Header("Animation Names")]
        [SerializeField] private string openAnimationName = "Door|Door_Open";
        [SerializeField] private string closeAnimationname = "Door|Door_Close";

        [SerializeField] private int timeToshowUI = 1;
        [SerializeField] private GameObject showDoorLockedUI = null;

        [SerializeField] private KeyInventory keyInventory = null;

        [SerializeField] private int waitTimer = 1;
        [SerializeField] private bool pauseInteraction = false;

        private void Awake()
        {
            doorAnim = gameObject.GetComponent<Animator>();
        }
        private IEnumerator PauseDoorInteraction()
        {
            pauseInteraction = true;
            yield return new WaitForSeconds(waitTimer);
            pauseInteraction = false;
        }

        public void PlayAnimation()
        {
            if (keyInventory.hasRedKey)
            {
                if(!doorOpen && !pauseInteraction)
                {
                    doorAnim.Play(openAnimationName, 0, 0.0f);
                    doorOpen = false;
                    StartCoroutine(PauseDoorInteraction());
                }
            }
            else
            {
                StartCoroutine(ShowDoorLocked());
            }
        }

        IEnumerator ShowDoorLocked()
        {
            showDoorLockedUI.SetActive(true);
            yield return new WaitForSeconds(timeToshowUI);
            showDoorLockedUI.SetActive(false);
        }
    }
}

