using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisCamera : MonoBehaviour
{
    [Header("References")]
    private Camera cam;
    public Transform target;

    [Header("Setup")]
    [Tooltip("How fast the Camera moves")]
    public float camspeed = 10f;
    [Tooltip("How far the player must be before the Camera starts scrolling")]
    public float startScroll = 20f;
    [Tooltip("How far the player must be while the camera is scrolling, to stay scrolling")]
    public float moveScroll = 10f;
    [Tooltip("0 is X, 1 is Y, 2 is Z. Controls what Axis the camera tracks")]
    public int axisChoice = 0; // 0 is X, 1 is Y, 2 is Z
    [Tooltip("Vector direction the cameara will move in, change this to make it go forward/back, up/down")]
    public Vector3 movedir = new Vector3(1,0,0);


    [Header("Debug")]
    public bool isScroll = false;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (axisChoice == 0)
        {

            float playerDist = Mathf.Abs(target.transform.position.x - cam.transform.position.x);


            if (playerDist > startScroll){ 
                isScroll = true;


                if(target.transform.position.x < cam.transform.position.x)
                {
                    transform.Translate(-movedir * camspeed * Time.deltaTime, Space.World);
                }
                else
                {
                    transform.Translate(movedir * camspeed * Time.deltaTime, Space.World);
                }
                    
            }else if (isScroll && playerDist > moveScroll)
            {
                if (target.transform.position.x < cam.transform.position.x)
                {
                    transform.Translate(-movedir * camspeed * Time.deltaTime, Space.World);
                }
                else
                {
                    transform.Translate(movedir * camspeed * Time.deltaTime, Space.World);
                }
            }
            else
            {
                isScroll = false;
            }

        }
        else if (axisChoice == 1) {

            float playerDist = Mathf.Abs(target.transform.position.y - cam.transform.position.y);


            if (playerDist > startScroll)
            {
                isScroll = true;


                if (target.transform.position.y < cam.transform.position.y)
                {
                    transform.Translate(-movedir * camspeed * Time.deltaTime, Space.World);
                }
                else
                {
                    transform.Translate(movedir * camspeed * Time.deltaTime, Space.World);
                }

            }
            else if (isScroll && playerDist > moveScroll)
            {
                if (target.transform.position.y < cam.transform.position.y)
                {
                    transform.Translate(-movedir * camspeed * Time.deltaTime, Space.World);
                }
                else
                {
                    transform.Translate(movedir * camspeed * Time.deltaTime, Space.World);
                }
            }
            else
            {
                isScroll = false;
            }



        }
        else
        {
            float playerDist = Mathf.Abs(target.transform.position.z - cam.transform.position.z);


            if (playerDist > startScroll)
            {
                isScroll = true;


                if (target.transform.position.z < cam.transform.position.z)
                {
                    transform.Translate(-movedir* camspeed * Time.deltaTime, Space.World);
                }
                else
                {
                    transform.Translate(movedir* camspeed * Time.deltaTime, Space.World);
                }

            }
            else if (isScroll && playerDist > moveScroll)
            {
                if (target.transform.position.z < cam.transform.position.z)
                {
                    transform.Translate(-movedir * camspeed * Time.deltaTime, Space.World);
                }
                else
                {
                    transform.Translate(movedir * camspeed * Time.deltaTime,Space.World);
                }
            }
            else
            {
                isScroll = false;
            }

        }
    }
}
