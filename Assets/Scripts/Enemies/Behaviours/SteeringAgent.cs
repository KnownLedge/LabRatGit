using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringAgent : MonoBehaviour
{
    #region declaration

    //AI - movements variables
    //the maximum speed the agent can move
    public float MaxVelocity { get; protected set; } = 10f;
    public Vector3 steeringVelocity { get; protected set; }
    public Vector3 currentVelocity { get; protected set; }

    //AI-time variables
    [Range(0.1f, 1f)]
    public float MaxTime;
    private float currTime;
    

    private List<SteeringBehaviour> Behaviours = new List<SteeringBehaviour>();

    #endregion

    private void Start() { }

    // Update is called once per frame
    private void Update()
    {
        currTime += Time.deltaTime;
        while (currTime >= MaxTime)
        {
            currTime -= MaxTime;
            CooperativeArbitration();
        }

        //update the position of the agent
        UpdateMovement();
        //Rotate the agent to the current currentTarget
        UpdateDirection(); 
    }

    protected virtual void CooperativeArbitration()
    {

        steeringVelocity = new Vector3(0, 0, 0);

        GetComponents<SteeringBehaviour>(Behaviours);

        foreach (SteeringBehaviour currentBehaviour in Behaviours)
        {
            if (currentBehaviour.enabled)
            {
                steeringVelocity += currentBehaviour.UpdateForce(this);

            }

        }
        
    }


    //this function when called will move the agent towards the currentTarget
    private void UpdateMovement()
    {
        var LimitVelocity = limitVelocity(steeringVelocity, MaxVelocity * Time.deltaTime);
        steeringVelocity -= LimitVelocity;

        currentVelocity += LimitVelocity;
        currentVelocity = limitVelocity(currentVelocity, MaxVelocity);

        transform.position += currentVelocity * Time.deltaTime;
    }

    //this function when called will rotate the agent towards the currentTarget
    private void UpdateDirection()
    {
        if (currentVelocity.sqrMagnitude > 0.01f)
        {
            transform.forward = Vector3.Normalize(new Vector3(currentVelocity.x, currentVelocity.y, currentVelocity.z));
        }
    }


    //when called this function will take the current velocity and limit it to the max velocity
    private Vector3 limitVelocity(Vector3 vector, float mag)
    {
        if (vector.sqrMagnitude > mag * mag)
        {
            vector.Normalize();
            vector *= mag;
        }
        return vector;
    }
}
