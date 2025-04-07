using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringLevelTeleport : MonoBehaviour
{
    public string level = "";

    public LevelSelectController levelControl;
    private void Start()
    {

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            levelControl.LevelTransition(level);
        }
    }
}
