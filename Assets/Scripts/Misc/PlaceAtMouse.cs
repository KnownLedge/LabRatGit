using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceAtMouse : MonoBehaviour
{
   [SerializeField] private Camera mainCamera;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit)){
            transform.position = raycastHit.point;
        }
    }
}
