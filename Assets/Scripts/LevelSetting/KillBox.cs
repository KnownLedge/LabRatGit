using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    public Transform spawnPoint; // Where the rat will respawn at
    private GameObject player; // player reference

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void OnTriggerEnter(Collider col) // when collision with this object, sets players position to spawnpoints position
    {
        if (col.gameObject.tag == "Player")
        {
            Ratmovement ratMove = col.gameObject.GetComponent<Ratmovement>();
            player.gameObject.transform.position = spawnPoint.position;
            ratMove.backLeg.position = spawnPoint.position;
        }
    }
}
