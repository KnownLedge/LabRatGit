using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMazeEnter : MonoBehaviour
{
    public Scenemanager sceneControl;
    public Transform[] platforms;
    

    void Start()
    {
        if (Scenemanager.scenePhase > 0){
          Scenemanager.scenePhase = Mathf.Clamp(Scenemanager.scenePhase, 1, platforms.Length);
            platforms[Scenemanager.scenePhase - 1].gameObject.SetActive(true);
        }

    }

}
