using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisCamera : MonoBehaviour
{
    private Camera cam;
    public float camspeed = 10f;
    public Transform target;
    public int axisChoice = 0; // 0 is X, 1 is Y, 2 is Z
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (axisChoice == 0)
        {
          

        }else if (axisChoice == 1) {

        }
        else
        {

        }
    }
}
