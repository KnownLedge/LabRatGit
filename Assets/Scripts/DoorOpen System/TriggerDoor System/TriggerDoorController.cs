using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnim = null;

    [SerializeField] private bool openTrigger = false;
    [SerializeField] private bool closeTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(openTrigger)
            {
                doorAnim.Play("DoorOpen", 0, 0.0f);
                gameObject.SetActive(false);
            }
            else if(closeTrigger)
            {
                doorAnim.Play("DoorClose", 0, 0.0f);
                gameObject.SetActive(false);
            }
        }
    }
}
