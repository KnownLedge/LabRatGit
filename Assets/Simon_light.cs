using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class Simon_light : MonoBehaviour
{
    public static Simon_light Instance;
    public float waitTime;
    private void Awake()
    {
        Instance = this;
    }
    public void SetColour(Color color)
    {
        var light = GetComponent<Light>();
        Debug.Log(color + " " + light.color);
        float lerpRed = Mathf.Lerp(light.color.r, color.r, 1f);
        float lerpBlue = Mathf.Lerp(light.color.b, color.b, 1f);
        float lerpGreen = Mathf.Lerp(light.color.g, color.g, 1f);
        Color newColor = new Color(lerpRed, lerpGreen, lerpBlue);
        light.color = newColor;
    }

}
