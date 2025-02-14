using UnityEngine;

public class Simon_Switch : Simon_light
{
    private Material Mat;
    private Light buttonlight;
    private enum ButtonState
    {
        on,
        off
    }
    
    private ButtonState state;

    //the game will crash if not included
    protected override void Start() 
    {
        gameObject.SetActive(false);
    }

    protected override void Update() { }

    protected override void IStart(Color color)
    {  
        gameObject.SetActive(true);
        Mat = GetComponent<MeshRenderer>().material;
        light = GetComponentInParent<Light>();
        if(buttonlight == null)
            buttonlight = gameObject.AddComponent<Light>(); 
        buttonlight.color = color;
        buttonlight.intensity = 5.5f;
        Mat.SetColor("_Color",color);
        Mat.SetColor("_EmissionColor", color);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (state == ButtonState.on)
        {
            buttonlight.enabled = false;
            light.color = Mat.color;
            Simon_light simon_Light = transform.parent.GetComponent<Simon_light>();
            simon_Light.AddToList(name);
            SetFalse();
        }
    }
    protected override void Finish()
    {
        buttonlight.enabled = false;
        Mat.SetColor("_EmissionColor", new Color(0,0,0,0));
    }
    protected void OnTriggerExit(Collider other)
    {
        buttonlight.enabled = true;
        SetTrue();
    }
    public void SetFalse()
    {
        SetState(ButtonState.off);
    } 
    public void SetTrue()
    {
        SetState(ButtonState.on);
    }
    private ButtonState SetState(ButtonState newstate)
    {
        state = newstate;
        return state;
    }


    
}
