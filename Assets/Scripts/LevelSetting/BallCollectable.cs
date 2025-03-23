using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollectable : MonoBehaviour
{
    [SerializeField] private GameObject collectable; 

    private void Start()
    {
        collectable.SetActive(false);
    }

    //When ball enters the trigger, the collectable will be set to active
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            collectable.SetActive(true);
        }
    }
}

