using System.Collections;
using UnityEngine;

public class PathFinderBehaviour : SteeringBehaviour
{
    //this script is a simple arival behaviour 
    float ARadius = 10.0f;
    public float D2T;
    public float resetTime;
    bool close;
    int moveSpeed;

    IEnumerator coroutine;

    int targetIndex;
    public Transform[] Pathtarget;
    public Transform currentTargetPosition;

    public override Vector3 UpdateForce(SteeringAgent steeringAgent)
    {
        if(currentTargetPosition == null)
        {
            currentTargetPosition = Pathtarget[0];
        }
        //Get the desired velocity of for the seek functionity and limited to the agents max speed
        desiredVelocity = Vector3.Normalize(currentTargetPosition.position - transform.position);

        float distance = (currentTargetPosition.position - transform.position).magnitude;
        if (distance < ARadius)
        {
            desiredVelocity *= steeringAgent.MaxVelocity * (distance / ARadius);
        }
        else {
            desiredVelocity *= steeringAgent.MaxVelocity;
        }

        //calculate the steering velocity 
        steeringVelocity = desiredVelocity - steeringAgent.currentVelocity;
        if (distance <= D2T && !close)
        {
            coroutine = startTime(resetTime);
            StartCoroutine(coroutine);
            close = !close;
        }
        return steeringVelocity;
    }

    private IEnumerator startTime(float resetTime)
    {
        yield return new WaitForSeconds(resetTime);
        SetNewTarget();
    }

    public void SetNewTarget() 
    {
        targetIndex++;
        if(targetIndex >= Pathtarget.Length)
        {
            targetIndex = 0;
        }
        currentTargetPosition = Pathtarget[targetIndex];
        close = !close;

    }
}
