using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreData : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject collectable;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (collectable == null)
        {
            collectable = GameObject.FindWithTag("Collectable");
        }
        DataRestorer();
    }

    private void DataRestorer()
    {
        if(player != null)

        {
            player.transform.localPosition = new Vector3(11.319f,0.658f,19.75f);
        }

        Destroy(collectable.gameObject);

        Debug.Log($"Restoring rat position: {player.transform.localPosition}");
    }
}
