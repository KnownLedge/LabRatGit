using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterForce : MonoBehaviour
{

    public float pushForce = 5f;
    private Rigidbody ratRb;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision occurred");
        if (collision.gameObject.GetComponent<Ratmovement>())
        {
            ratRb = collision.gameObject.GetComponent<Rigidbody>();
            ratRb.AddForce(0, pushForce, 0, ForceMode.Force);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.GetComponent<Ratmovement>())
        {
            ratRb.AddForce(0, pushForce, 0, ForceMode.Force);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<Ratmovement>())
        {
            ratRb.velocity = new Vector3(ratRb.velocity.x, 0, ratRb.velocity.z);
        }
    }

}
