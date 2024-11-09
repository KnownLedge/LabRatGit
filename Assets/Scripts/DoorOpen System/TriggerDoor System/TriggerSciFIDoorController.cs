using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSciFIDoorController : MonoBehaviour
{
    [SerializeField] private Animator sciFiDoorAnim = null;

    [SerializeField] private bool openTrigger = false;
    [SerializeField] private bool closeTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(openTrigger)
            {
                sciFiDoorAnim.Play("SciFiDoorOpen", 0, 0.0f);
                gameObject.SetActive(false);
            }
            else if(closeTrigger)
            {
                sciFiDoorAnim.Play("SciFiDoorClose", 0, 0.0f);
                gameObject.SetActive(false);
            }
        }
    }
}
