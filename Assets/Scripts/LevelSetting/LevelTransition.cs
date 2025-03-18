using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    public int level = 1;

     public LevelSelectController levelControl;
    private void Start()
    {
        
    }
    void OnTriggerEnter(Collider col) 
    {
        Debug.Log("FIRED");
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("TRIGGERED");
            levelControl.LevelTransition(level);
        }
    }
}
