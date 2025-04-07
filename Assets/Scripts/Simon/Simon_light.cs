using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simon_light : MonoBehaviour
{
    protected new Light light;

    [SerializeField]
    private int outputRange;

    [SerializeField, Range(1, 5)]
    private int cycles;

    private int currentcycles;

    private List<Color> colorInput = new List<Color>(); // a list of the player switch inputs
    private IEnumerator coroutine;
    private List<Color> colors = new List<Color>(); // a list of colours for Simon to use
    private readonly List<Color> output = new List<Color>(); // a list to store the outputs

    private GameObject[] switches = new GameObject[4]; // an array of switches
    private bool isOutputing;
    private int SimonIndex;
    public GameObject[] barriers;

    [Header("Screens")]
    [SerializeField] private GameObject greenTickScreen;
    [SerializeField] private GameObject redCrossScreen;

    private enum States // the available States
    {
        off,
        start,
        right,
        wrong,
        input,
        wInput,
        output,
        win
    }

    [SerializeField]
    private States state; // the current light state

    private void Awake()
    {
        light = GetComponent<Light>();
        colors = new List<Color>
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow
        };

        switches[0] = GameObject.Find("Red-light");
        switches[1] = GameObject.Find("Green-light");
        switches[2] = GameObject.Find("Blue-light");
        switches[3] = GameObject.Find("Yellow-light");
        if (barriers != null)
        {
            for (int i = 0; i < barriers.Length; i++)
            {
                barriers[i].SetActive(false);
            }
        }
    }

    private void SetOutput()
    {
        if (output != null)
        {
            output.Clear();
        }

        for (int i = 0; i < outputRange; i++)
        {
            output.Add(colors[Random.Range(0, colors.Count)]);
        }
    }

    // set/changes the colours
    protected virtual void SetColour(Color Lightcolor)
    {
        if (light.enabled != true)
            light.enabled = true;
        light.color = Lightcolor;
    }

    protected virtual void Start()
    {
        foreach (GameObject Switch in switches)
        {
            Switch.AddComponent<Simon_Switch>();
        }
    }

    protected virtual void Update()
    {
        if (SimonIndex == outputRange + 1)
        {
            SimonIndex = 0;
            light.enabled = false;
            StopCoroutine(coroutine);
            coroutine = null;
            setState(States.input);
            isOutputing = !isOutputing;
            for (int i = 0; i < switches.Length; i++)
            {
                switches[i].GetComponent<Simon_Switch>().SetTrue();
            }
        }

        if (currentcycles == cycles)
        {
            setState(States.win);
        }

        switch (state) // determines the state
        {


            //default state
            default:
                setState(States.off);
                break;

            //off state
            case States.off:
                light.enabled = false;
                break;

            //start state
            case States.start:
                SetOutput();
                setState(States.output);
                break;
            //output state
            case States.output:
                if (!isOutputing)
                {
                    coroutine = OutPut(1.0f);
                    StartCoroutine(coroutine);
                    isOutputing = !isOutputing;
                }
                for (int i = 0; i < switches.Length; i++)
                {
                    switches[i].GetComponent<Simon_Switch>().IStart(colors[i]);
                    switches[i].GetComponent<Simon_Switch>().SetFalse();
                }
                break;

            case States.input:
                break;

            case States.right: // if the inputs are right then simon outputs are reset and the cycle increments
                outputRange++;
                currentcycles++;
                setState(States.start);
                colorInput.Clear();
                ShowCorrectScreen();
                break;

            case States.wrong: // if the inputs are wrong then simon outputs are reset and the cycle doesn't increment
                setState(States.start);
                colorInput.Clear();
                ShowWrongScreen();
                break;

            case States.win:
                light.enabled = false;
                for (int i = 0; i < switches.Length; i++)
                {
                    switches[i].GetComponent<Simon_Switch>().Finish();
                }
                SimonIndex = 0;
                outputRange -= currentcycles;
                currentcycles = 0;
                setState(States.off);
                Finish();
                break;
        }
    }

    protected virtual void Finish()
    {
        if (barriers != null)
        {
            for (int i = 0; i < barriers.Length; i++)
            {
                barriers[i].SetActive(false);
            }
        }
    }

    private void setState(States newState) // set the state of the Simon
    {
        Debug.Log($"new State {newState}");
        state = newState;
    }

    private IEnumerator OutPut(float Duration) // output the colours to the player in order of the output list
    {
        while (SimonIndex < outputRange + 1) // a recursive loop
        {
            light.enabled = false; // disable the light
            yield return new WaitForSeconds(Duration); // wait for the duration passed through the function
            if (SimonIndex < outputRange)
            {
                SetColour(output[SimonIndex]);
                light.enabled = true;
            }
            SimonIndex++;
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator WaitTime(float Duration)
    {
        yield return new WaitForSeconds(Duration);
        setState(States.right);
    }

    protected virtual void IStart(Color color) { } // when the derived are enabled, this function is called

    protected virtual void OnTriggerEnter(Collider other) // collision trigger
    {
        if (state == States.off)
        {
            setState(States.start);
            for (int i = 0; i < barriers.Length; i++)
            {
                barriers[i].SetActive(true);
            }
        }
    }

    public void AddToList(string name)
    {
        for (int i = 0; i < switches.Length; i++)
        {
            if (name == switches[i].name)
            {
                SetColour(colors[i]);
                colorInput.Add(colors[i]);
                CheckInputs();
                break;
            }
        }
    }

    private void CheckInputs()
    {
        for (int i = 0; i < colorInput.Count; i++)
        {
            if (colorInput[i] != output[i])
            {
                Debug.Log($"false at:{i}");
                setState(States.wrong);
            }
        }
        bool IsMatching = false;
        if (colorInput.Count == outputRange)
        {
            for (int i = 0; i < colorInput.Count; i++)
            {
                if (colorInput[i] == output[i])
                {
                    Debug.Log($"correct at: {i}");
                    IsMatching = true;
                }
            }
            if (IsMatching == true)
            {
                coroutine = WaitTime(1f);
                StartCoroutine(coroutine);
            }
        }
    }

    private void ShowCorrectScreen()
    {
        // Show green tick and hide red cross
        greenTickScreen.SetActive(true);
        redCrossScreen.SetActive(false);
        StartCoroutine(HideScreensAfterDelay(1f));
    }

    private void ShowWrongScreen()
    {
        // Show red cross and hide green tick
        greenTickScreen.SetActive(false);
        redCrossScreen.SetActive(true);
        StartCoroutine(HideScreensAfterDelay(1f));
    }

    private IEnumerator HideScreensAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        greenTickScreen.SetActive(false);
        redCrossScreen.SetActive(false);
    }
}
