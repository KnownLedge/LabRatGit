using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public float openTime; //How long door takes to open
    public float angleChange = 90f;
    private float lerpTimer;
    private Quaternion origRotation;
    void Start()
    {
        origRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float rotationval = Mathf.LerpAngle(origRotation.y, origRotation.y +angleChange, lerpTimer);
        transform.rotation = Quaternion.Euler(origRotation.x,rotationval,origRotation.z);
        lerpTimer += Time.deltaTime / openTime;
    }
}
