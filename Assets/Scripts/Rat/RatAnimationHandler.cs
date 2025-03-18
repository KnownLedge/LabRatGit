using System.Collections;
using UnityEditor;

using System.Collections.Generic;
using UnityEngine;

public class RatAnimationHandler : MonoBehaviour
{
    // Script for animating rat, might be better just doing this in ratmovement if this gets too complex

    public Animator animControl;
    public float speedMult = 1;
    public Rigidbody rb;
    private float forwardSpeed = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 LocalVelocity = (transform.InverseTransformDirection(rb.velocity));
        animControl.SetFloat("ForwardSpeed",  LocalVelocity.z * speedMult);
    }
}
