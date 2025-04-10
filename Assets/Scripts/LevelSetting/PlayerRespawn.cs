using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] FadeManager fadeManager;
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
            StartCoroutine(respawnCoroutine());
        }
    }

    IEnumerator respawnCoroutine()
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(fadeManager.RespawnFade());
        Ratmovement ratMove = player.GetComponent<Ratmovement>();
        player.gameObject.transform.position = spawnPoint.position;
        ratMove.backLeg.position = spawnPoint.position;
        yield return new WaitForSeconds(1f); 
        player.GetComponent<Rigidbody>().isKinematic = false;
    }

}