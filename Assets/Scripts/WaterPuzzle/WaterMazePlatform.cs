using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMazePlatform : MonoBehaviour
{

    public bool onPlatform = false;
    public float lerpTimer = 0;
    public Transform platform;
    public Vector3 startPos;
    public float platHeight = 20f;
   // public float winTimer = 2f;
    public Scenemanager sceneControl; //Really shouldn't be using this for these objects, but I want this done for vertical slice

    private Renderer meshRend;
    private bool mazeComplete = false;

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
       lerpTimer = Mathf.Clamp(lerpTimer, 0.0f, 0.5f);
        //Debug.Log();

        if(lerpTimer >= 0.5){
            meshRend.enabled = true;
            if(!mazeComplete){
            Scenemanager.scenePhase += 1;
            mazeComplete = true;
            }
            sceneControl.SceneTransition();
        }else if (lerpTimer == 0){
            meshRend.enabled = false;
        }

        platform.position = Vector3.Lerp(startPos, transform.position, lerpTimer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Ratmovement>())
        {
            other.gameObject.GetComponentInParent<Rigidbody>().isKinematic = true;
            //other.gameObject.transform.parent = transform;
            Vector3 boxShape = GetComponent<BoxCollider>().bounds.center;
            boxShape.y = platHeight;
            other.gameObject.transform.parent.position = boxShape;
            onPlatform = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Ratmovement>()) {
            onPlatform = false;
        }
    }

}
