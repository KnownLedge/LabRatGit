using System.Collections.Generic;
using UnityEngine;
public class Simon_light : MonoBehaviour
{
    protected new Light light;
    private List<Color> colors = new List<Color>();// a list of colours for simon to use
    private GameObject[] switches = new GameObject[4];
    private List<Color> ColorOrder = new List<Color>();
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
    }
    protected virtual void SetColour(Color Lightcolor)
    {
        Debug.Log(Lightcolor);
        light.color = Lightcolor;
    }
    protected virtual void Start()
    {
        foreach (GameObject Switch in switches)
        {
            Switch.AddComponent<Simon_Switch>();
        }

        for (int i = 0; i < switches.Length; i++)
        {
            switches[i].GetComponent<Simon_light>().IStart(colors[i]);
        }

    }

    protected virtual void IStart(Color color) { }
    private void Update()
    {
        Iupdate();
    }

    protected virtual void Iupdate(){}

   

    protected virtual void OnTriggerEnter(Collider other)
    {
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        resetLightColor();
    }
    private void resetLightColor() {
        SetColour(Color.white);
    }

}
