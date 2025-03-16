using System.Collections.Generic;
using UnityEngine;

public class RPS : MonoBehaviour
{
   

    private MeshRenderer _Screen;
    public static RPS _instance;

    private List<string> _Ops = new List<string>
    {
        "rock",
        "paper",
        "scissors"
    };
    private enum states
    {
        enter,
        Exit
    }

    private states CurrentState;

    private void Awake()
    {
        _instance = this;
        CurrentState = states.enter;
        _Screen = GameObject.Find("Screen").GetComponent<MeshRenderer>();//get the screen game object
        SetAIOppent();
    }

    //when called a name is passed
    public void CheckInput(string name)
    {
        //check if Oppent is null
        if (Oppent != null)
        {// this is terrible but this is a prototype
            if (name == Oppent)
            {
                _Screen.material.SetColor("_Color", Color.black);
            }
            if (name == "rock" && Oppent == "paper")
            {
                _Screen.material.SetColor("_Color", Color.red);
            }
            if (name == "rock" && Oppent == "scissors")
            {
                _Screen.material.SetColor("_Color", Color.green);

            }
            if (name == "paper" && Oppent == "scissors")
            {
                _Screen.material.SetColor("_Color", Color.red);
            }
            if (name == "paper" && Oppent == "rock")
            {
                _Screen.material.SetColor("_Color", Color.green);

            }
            if (name == "scissors" && Oppent == "rock")
            {
                _Screen.material.SetColor("_Color", Color.red);
            }
            if (name == "scissors" && Oppent == "paper")
            {
                _Screen.material.SetColor("_Color", Color.green);

            }
        }
    }

    public void DO(string name)
    {
        if (CurrentState == states.enter)
        {
            SetStateOff();
            CheckInput(name);
        }
    }

    #region AI Oppent
    //Set the current state
    private states SetState(states states)
    {
        CurrentState = states;
        return CurrentState;
    }
    public void Reset()
    {
        _Screen.material.SetColor("_Color", Color.white);
        SetAIOppent();
    }
    public void SetStateOn()
    {
        SetState(states.enter);
    }
    public void SetStateOff()
    {
        SetState(states.Exit);
    }

    private string Oppent;

    private void SetAIOppent()
    {
        SetStateOn();
        Oppent = _Ops[Random.Range(0,_Ops.Count)];
    }
    #endregion
}