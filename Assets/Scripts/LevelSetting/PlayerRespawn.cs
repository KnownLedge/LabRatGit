using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private FadeManager fadeManager;
    [SerializeField] private float threshold;
    [SerializeField] List<GameObject> checkpoints;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if (transform.position.y < threshold)
        {
            GetComponent<CharacterController>().enabled = false;
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            GetComponent<CharacterController>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Checkpoint"))
        { 
            //Ratmovement ratMove = col.gameObject.GetComponent<Ratmovement>();
            Vector3 vectorPoint = other.transform.position;
            player.gameObject.transform.position = vectorPoint;
            //ratMove.backLeg.position = vectorPoint;
        } 
        else if (other.gameObject.tag == "Player")
        {
            StartCoroutine(fadeManager.RespawnFade());
            Ratmovement ratMove = other.gameObject.GetComponent<Ratmovement>();
            Vector3 vectorPoint = other.transform.position;
            player.gameObject.transform.position = vectorPoint;
            ratMove.backLeg.position = vectorPoint;
        }
    }

}
