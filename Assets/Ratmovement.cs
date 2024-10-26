using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratmovement : MonoBehaviour
{
    public Rigidbody rb;
    Vector3 mousePos;

    public bool moveState = true;
    public bool isJump = false;


    public float moveSpeed = 20f;
    public float jumpPower = 600f;
    public float jumpForce = 16f;

    public enum jumpFreedom
    {
        Locked,
        SteerAllowed,
        SpeedControl,
        FreeMovement
    }

    public jumpFreedom jumpStyle = jumpFreedom.Locked;

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
        mousePos = Input.mousePosition; //Get mouse position from input

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        //Get cannon object position on screen through the camera
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        //Get the difference between the mouse position and cannon position

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        //Get the angle to the mouse position using maths I don't fully understand
        //  angle -= 90f;
        //The cannon defaults facing up, so we need to turn the angle to accomodate

        if (moveState || jumpStyle != jumpFreedom.Locked) //steer, speed and free can pass 
        {

            if (moveState || jumpStyle != jumpFreedom.SpeedControl) // steer and free can pass
            {

                transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
                

            }

            if (moveState || jumpStyle != jumpFreedom.SteerAllowed) // speed and free can pass
            {

                if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.W))
                {
                    //  transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);
                    rb.AddForce(transform.right * moveSpeed);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    transform.Translate(transform.forward * -moveSpeed * Time.deltaTime, Space.World);
                }
            }
        }
    
        if(Input.GetKeyDown(KeyCode.Space)){
           moveState = false;
            isJump = true;
           // float forcXDir = rb.velocity.x
           // float forcX = Mathf.InverseLerp(rb.velocity)
            rb.velocity = new Vector3(transform.right.x * jumpForce, jumpPower, transform.right.z * jumpForce);
        }

        if(Input.GetKeyDown(KeyCode.X)){
           moveState = true;
            isJump = false;
            }


        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed),rb.velocity.y,Mathf.Clamp(rb.velocity.z, -moveSpeed, moveSpeed));

    }

    void OnCollisionEnter(Collision collision)
    {
        isJump = false;
        moveState = true;

    }


}


/* Todo

Unlock rat rotation for easier ramp access (also makes the rat hop when flipped, explore this)
Lock rat rotation when jumping (make this an option, making the cube flip is funny)
Create three different settings for jump controls (locked, steering allowed, free, etc) DONE
Investigate different force movement to allow different jump settings to have more options (air steering is currently useless)
*/