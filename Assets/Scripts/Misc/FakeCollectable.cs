using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeCollectable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Destroy(gameObject);
        }
    }
}
