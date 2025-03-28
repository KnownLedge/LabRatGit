using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnter : MonoBehaviour
{
    public Scenemanager sceneControl;
    public Transform[] spawnPoints;
    private GameObject player; // player reference
    public Transform cameraRef;
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (Scenemanager.sceneCheckPoint > 0){
            player.transform.position = spawnPoints[Scenemanager.sceneCheckPoint - 1].position;
            cameraRef.position = new Vector3(spawnPoints[Scenemanager.sceneCheckPoint - 1].position.x,cameraRef.position.y,cameraRef.position.z);
        }

    }

}
