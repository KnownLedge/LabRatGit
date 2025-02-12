using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBodyRotation : MonoBehaviour
{
    [Header("Physics")]
    public Transform frontCapsule;
    public Transform backCapsule;

    [Header("Animation Body")]
    public Transform spine;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spine.position = frontCapsule.position;
   //     spine.rotation = frontCapsule.rotation;
       // spine.Rotate(0,90,0);
     //   spine.rotation = frontCapsule.rotation;
    // spine.LookAt();
    }
}
