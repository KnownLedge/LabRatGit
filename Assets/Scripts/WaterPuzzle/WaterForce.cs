using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterForce : MonoBehaviour
{

    public float pushForce = 5f;
    private Rigidbody ratRb;
    private Ratmovement ratMove;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision occurred");
        if (collision.gameObject.GetComponentInParent<Ratmovement>())
        {
            ratMove = collision.gameObject.GetComponentInParent<Ratmovement>();
            ratMove.isJump = true; //Prevent rat from jumping out of water
            ratMove.moveState = true; //Allow rat to move if they jumped into the water
            ratRb = ratMove.GetComponent<Rigidbody>();
            ratRb.AddForce(0, pushForce, 0, ForceMode.Force);
        }else{
            Debug.Log("Bzztt");
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<Ratmovement>())
        {
            ratRb.AddForce(0, pushForce, 0, ForceMode.Force);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<Ratmovement>())
        {
            ratRb.velocity = new Vector3(ratRb.velocity.x, 0, ratRb.velocity.z);
        }
    }

}
