using UnityEngine;

public class Simon_Switch : Simon_light
{
    private Material Mat;
    private Light buttonlight;

    protected override void Start() { }
    protected override void Iupdate() { }
    protected override void IStart(Color color)
    {
        Mat = GetComponent<MeshRenderer>().material;
        light = GetComponentInParent<Light>();
        buttonlight = gameObject.AddComponent<Light>();
        buttonlight.color = color;
        buttonlight.intensity = 5.5f;
        Mat.SetColor("_Color",color);
        Mat.SetColor("_EmissionColor", color);
    }

    protected override void SetColour(Color Lightcolor)
    {
       base.SetColour(Lightcolor);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        SetColour(Mat.color);
        buttonlight.enabled = false;
        base.OnTriggerEnter(other);

    }
    protected override void OnTriggerExit(Collider other)
    {
        buttonlight.enabled = true;
        base.OnTriggerExit(other);
    }
  
}
