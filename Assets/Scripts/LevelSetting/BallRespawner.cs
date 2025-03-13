using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRespawner : MonoBehaviour
{
    public Transform targetBall; // ball to track and respawn
    private Vector3 spawnPos; //Position ball starts at to respawn to
    void Start()
    {
        spawnPos = targetBall.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == targetBall)
        {
            targetBall.position = spawnPos;
        }
    }
}
