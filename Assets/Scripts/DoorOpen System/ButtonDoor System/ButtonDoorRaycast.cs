using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDoorRaycast : MonoBehaviour
{
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;

    private ButtonDoorController raycastedObj;

    [SerializeField] private KeyCode openDoorKey = KeyCode.E;
    private bool doOnce;

    private const string interactableTag = "DoorButton";

    private void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;

        if (Physics.Raycast(transform.position, fwd, out hit, rayDistance, mask))
        {
            if (hit.collider.CompareTag(interactableTag))
            {
                if(!doOnce)
                {
                    raycastedObj = hit.collider.GetComponent<ButtonDoorController>();
                }
                doOnce = true;

                if(Input.GetKeyDown(openDoorKey))
                {
                    raycastedObj.PlayAnimation();
                }
            }
        }
    }

}
