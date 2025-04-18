using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RPS : MonoBehaviour
{
    private MeshRenderer _Screen;
    public bool glassScreen; //Whether the rps uses the glass monitor or just a material color
    public int cycles,CurCycle;
    private int Correct;
    public static RPS _instance;
    public GameObject SimonCollectable;
    public GameObject opponentCollectable;
    private GameObject wrongScreen;
    private GameObject tiedScreen;
    private GameObject correctScreen;

    public GameObject aiPlayer;
    private AIRatMove aiMovement;
    public List<Transform> RockRoute;
    public List<Transform> PaperRoute;
    public List<Transform> ScissorRoute;

    public List<float> ratExitTimings;

    public List<Transform> firstExitRoute;
    public List<Transform> secondExitRoute;
    public List<Transform> LastExitRoute; //These should not be publics, but im cutting corners

    public DoorScript playerDoor;
    public DoorScript opponentDoor;
    public DoorButton playerButton;
    public DoorButton opponentButton;

    public AiTrigger exitAI;

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

        if (aiPlayer != null) { 
        aiMovement = aiPlayer.GetComponent<AIRatMove>();
        }
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
        Reset();
        if (SimonCollectable != null)
            SimonCollectable.SetActive(false);
        if(opponentCollectable != null)
            opponentCollectable.SetActive(false );
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
                CurCycle--;
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
            if (CurCycle == cycles)
            {
                if (Correct > cycles / 2)
                {
                    if (SimonCollectable != null)
                        SimonCollectable.SetActive(true);

                    playerDoor.enabled = true;
                    playerButton.enabled = true;
                    exitAI.enabled = true;
                }
                else
                {
                    if (opponentCollectable != null)
                        opponentCollectable.SetActive(true);
                    opponentDoor.enabled = true;
                    opponentButton.enabled = true;
                    StartCoroutine(ratExitPath());
                }
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
        setAIPath(Oppent);
    }

    private void setAIPath(string choice)
    {
        if (CurCycle < cycles)
        {
            if (choice == "rock")
            {
                aiMovement.wayPoints = RockRoute;


            }
            else if (choice == "paper")
            {
                aiMovement.wayPoints = PaperRoute;
            }
            else
            {
                aiMovement.wayPoints = ScissorRoute;
            }
            aiMovement.restartPath();
        }
    }

    IEnumerator ratExitPath()
    {

        //Destroy route transforms for button so rat can't get confused and continue going to them.



        aiMovement.wayPoints = firstExitRoute;
        aiMovement.restartPath();
        yield return new WaitForSeconds(ratExitTimings[0]);
        aiMovement.wayPoints = secondExitRoute;
        aiMovement.restartPath();
        yield return new WaitForSeconds(ratExitTimings[1]);
        aiMovement.wayPoints = LastExitRoute;
        aiMovement.restartPath();
        yield return new WaitForSeconds(ratExitTimings[2]);
    }
    #endregion
}