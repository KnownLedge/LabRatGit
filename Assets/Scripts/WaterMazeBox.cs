using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMazeBox : MonoBehaviour
{
    public Scenemanager sceneControl;
    public int respawnCheckpoint;
    private void Start()
    {
        
    }
    void OnTriggerEnter(Collider col) // when collision with this object, sets players position to spawnpoints position
    {
        if (col.gameObject.tag == "Player")
        {
            Scenemanager.sceneCheckPoint = respawnCheckpoint;
            sceneControl.SceneTransition();
        }
    }
}