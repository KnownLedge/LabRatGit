using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour
{
    protected Vector3 steeringVelocity;
    protected Vector3 desiredVelocity;
    public abstract Vector3 UpdateForce(SteeringAgent steeringAgent);

    public virtual void Start() {/*needed for enable bool*/ }
    public virtual void Update() {}
   
}
