using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    protected enum States
    {
        Default,
        chaser,
    }
    private States currentState;

    public SteeringBehaviour[] behaviours;
    public AI_Sight sight;

    void Start()
    {
        currentState = States.Default;
        sight = gameObject.GetComponent<AI_Sight>();
    }

    void Update()
    {
     
        switch (currentState)
        {
            case States.Default:
                behaviours[0].enabled = true; 
                behaviours[1].enabled = false; 
                break;
            case States.chaser:
                behaviours[0].enabled = false;
                behaviours[1].enabled = true;
                break; 
        }

        if (sight.playerSpotted == true)
        {
            ChaserState();
        }
        else 
        {
            DefaultState();
        }
    }

    void ChaserState()
    {
        currentState = States.chaser;
    }
    void DefaultState()
    {
        currentState = States.Default;
    }
}
