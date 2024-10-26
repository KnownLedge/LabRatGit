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
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(-transform.right * camspeed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(transform.right * camspeed * Time.deltaTime, Space.World);
        }



    }
}
