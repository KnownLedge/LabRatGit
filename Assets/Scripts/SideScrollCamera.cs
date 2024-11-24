using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrollCamera : MonoBehaviour
{
    private Camera cam;
    public float camspeed = 10f;
    void Start()
    {
       cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A)) //move left
        {
            transform.Translate(-transform.right * camspeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.D)) //move right
        {
            transform.Translate(transform.right * camspeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.Q)) //move up
        {
            transform.Translate(transform.up * camspeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.E)) //move down
        {
            transform.Translate(-transform.up* camspeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.Z)) //move forwards
        {
            transform.Translate(transform.forward * camspeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.C)) //move backwards
        {
            transform.Translate(-transform.forward * camspeed * Time.deltaTime, Space.World);
        }
    }
}
