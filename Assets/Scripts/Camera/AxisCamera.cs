using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisCamera : MonoBehaviour
{
    [Header("References")]
    private Camera cam;
    public Transform target;

    [Header("Setup")]
    public float camspeed = 10f;
    public float startScroll = 20f;
    public float moveScroll = 10f;
    public int axisChoice = 0; // 0 is X, 1 is Y, 2 is Z
    public Vector3 movedir = new Vector3(1, 0, 0);

    [Header("Debug")]
    public bool isScroll = false;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (axisChoice == 0) // Track X-axis only
        {
            float playerDist = Mathf.Abs(target.transform.position.x - cam.transform.position.x);

            if (playerDist > startScroll)
            {
                isScroll = true;

                if (target.transform.position.x < cam.transform.position.x)
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

            // Lock the Z position
            Vector3 newPosition = transform.position;
            newPosition.z = 0f; // Set Z position to a fixed value (change this if needed)
            transform.position = newPosition;
        }
    }
}
