using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatUnBackBreaker : MonoBehaviour
{
    // This script fixes the rats animation when not in the build
    public Animator ratAnimator;
    public bool thingDone = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!thingDone){
                ratAnimator.applyRootMotion = true;
         //ratAnimator.applyRootMotion = false;
    }
    }
}
