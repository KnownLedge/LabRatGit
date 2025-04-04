using System.Collections.Generic;
using UnityEngine;

public class RPS : MonoBehaviour
{
    private MeshRenderer _Screen;
    public bool glassScreen; //Whether the rps uses the glass monitor or just a material color
    public int cycles,CurCycle;
    private int Correct;
    public static RPS _instance;
    public GameObject SimonCollectable;
    private GameObject wrongScreen;
    private GameObject tiedScreen;
    private GameObject correctScreen; 

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
        if (glassScreen)
        {
            wrongScreen = GameObject.Find("Wrong Screen");
            tiedScreen = GameObject.Find("Tie Screen");
            correctScreen = GameObject.Find("Correct Screen");
            // ^ I am very sorry - Ryan
        }
        else
        {
            _Screen = GameObject.Find("Screen").GetComponent<MeshRenderer>();//get the screen game object
        }
            SetAIOppent();
        if (SimonCollectable != null)
            SimonCollectable.SetActive(false);
    }

    //when called a name is passed
    private void CheckInput(string name)
    {
        //check if Oppent is null
        if (Oppent != null && CurCycle < cycles)
        {// this is terrible but this is a prototype
            if (name == Oppent)
            {
                SetScreen(1); //Tie
            }
            if (name == "rock" && Oppent == "paper")
            {
                SetScreen(0); // Wrong
            }
            if (name == "rock" && Oppent == "scissors")
            {
                SetScreen(2); //Right
                Correct++;

            }
            if (name == "paper" && Oppent == "scissors")
            {
                SetScreen(0); // Wrong
            }
            if (name == "paper" && Oppent == "rock")
            {
                SetScreen(2); //Right
                Correct++;
            }
            if (name == "scissors" && Oppent == "rock")
            {
                SetScreen(0); // Wrong
            }
            if (name == "scissors" && Oppent == "paper")
            {
                SetScreen(2); //Right
                Correct++;
            }
            CurCycle++;
            if (Correct == cycles)
            {
                if (SimonCollectable != null)
                    SimonCollectable.SetActive(true);
            }
        }
    }

    private void SetScreen(int result)
    {
        if (glassScreen)
        {
            if (result == 0)
            {
                wrongScreen.SetActive(true);
                tiedScreen.SetActive(false);
                correctScreen.SetActive(false);
            }
            else if (result == 1)
            {
                wrongScreen.SetActive(false);
                tiedScreen.SetActive(true);
                correctScreen.SetActive(false);
            }
            else
            {
                wrongScreen.SetActive(false);
                tiedScreen.SetActive(false);
                correctScreen.SetActive(true);
            }
        }
        else
        {
            if(result == 0)
            {
                _Screen.material.SetColor("_Color", Color.red);
            }else if (result == 1)
            {
                _Screen.material.SetColor("_Color", Color.black);
            }
            else
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
        if (glassScreen)
        {
            wrongScreen.SetActive(false);
            tiedScreen.SetActive(false);
            correctScreen.SetActive(false);
        }
        else
        {
            _Screen.material.SetColor("_Color", Color.white);
        }
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

    public string Oppent;

    private List<string> _Ops = new List<string>
    {
        "rock",
        "paper",
        "scissors"
    };

    private void SetAIOppent()
    {
        SetStateOn();
        Oppent = _Ops[Random.Range(0,_Ops.Count)];
    }
    #endregion
}