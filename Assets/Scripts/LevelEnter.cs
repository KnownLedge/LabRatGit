using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnter : MonoBehaviour
{
    public Scenemanager sceneControl;
    public Transform[] spawnPoints;
    private GameObject player; // player reference
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("SCENE CHECKPOINT ISSSS: " + Scenemanager.sceneCheckPoint );
        if (Scenemanager.sceneCheckPoint > 0){
            player.transform.position = spawnPoints[Scenemanager.sceneCheckPoint - 1].position;
        }

    }

}
