using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simon_Switch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            var Mat = GetComponent<MeshRenderer>();
            Color MatColour = Mat.material.color;
            Simon_light.Instance.SetColour(MatColour);
        }
    }
}
