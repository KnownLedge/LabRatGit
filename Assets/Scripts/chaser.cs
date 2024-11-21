using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaser : SteeringBehaviour
{
    public Transform targetPosition;

    //this script will cause the attached agent to PURSUE the player 
   public override Vector3 UpdateForce(SteeringAgent steeringAgent)
    {
        desiredVelocity = Vector3.Normalize(targetPosition.position - transform.position) * steeringAgent.MaxVelocity;
        steeringVelocity = desiredVelocity - steeringAgent.currentVelocity;
        return steeringVelocity;
    }
}
