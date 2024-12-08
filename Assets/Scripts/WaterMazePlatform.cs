using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMazePlatform : MonoBehaviour
{

    public bool onPlatform = false;
    public float lerpTimer = 0;
    public Transform platform;
    public Vector3 startPos;

    private Renderer meshRend;

    private void Start()
    {
      //  platform = GetComponentInChildren<Transform>();
      meshRend = GetComponent<Renderer>();
        startPos = platform.position;
    }

    private void FixedUpdate()
    {
        if (onPlatform)
        {
            lerpTimer += 3f * Time.deltaTime;
        }
        else
        {
            lerpTimer -= (3f * Time.deltaTime);
        }
       lerpTimer = Mathf.Clamp(lerpTimer, 0.0f, 1.0f);
        //Debug.Log();

        if(lerpTimer == 1){
            meshRend.enabled = true;
        }else if (lerpTimer == 0){
            meshRend.enabled = false;
        }

        platform.position = Vector3.Lerp(startPos, transform.position, lerpTimer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ratmovement>())
        {
            Debug.Log("Overlap Ocurred");
            onPlatform = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Ratmovement>()) {
            onPlatform = false;
        }
    }

}
