using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StressTrigger : MonoBehaviour
{
    public StressSounds soundManager;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<Ratmovement>())
        {
            soundManager.EnableHeartBeat();
        }

    }

        private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<Ratmovement>())
        {
            
            //soundManager.DisableHeartBeat();
        }
    }
}
