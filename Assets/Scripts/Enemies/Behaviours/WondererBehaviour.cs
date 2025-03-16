using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WondererBehaviour : SteeringBehaviour
{

    public float radius;
    bool onRoute;

    Vector3 target;


    public override Vector3 UpdateForce(SteeringAgent steeringAgent) 
    {
        desiredVelocity = Move();
        steeringVelocity = desiredVelocity - steeringAgent.currentVelocity;
        return steeringVelocity; 
    }

   
    public override void Update()
    {

        if (!onRoute)
        {
            SetRoute();
        }

        Debug.DrawLine(transform.position, target, Color.red);

        if (onRoute)
        {
            if (target != null)
            {
                Move();
            }
            if ((transform.position - target).magnitude <= 1f)
                SetRoute();
        }

    }

    Vector3 Move()
    {
        Vector3 dir = target - transform.position;
        return dir;
    }




    //returns a new target vector value 
    Vector3 SetRoute()
    {

        float x = Mathf.Sin(Random.Range(-Mathf.PI, Mathf.PI)) * radius;
        float z = Mathf.Cos(Random.Range(-Mathf.PI, Mathf.PI)) * radius;

        target = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        onRoute = !onRoute;
        return target;
    }
}
