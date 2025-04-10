using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    private bool isEnabled;
    public DoorScript connectedDoor;
    void Start()
    {
        isEnabled = true;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (isEnabled) {
            connectedDoor.enabled = true;
        }
    }

}
