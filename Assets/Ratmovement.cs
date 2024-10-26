using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratmovement : MonoBehaviour
{
    private Rigidbody rb; //Player rigidbody component
    private RigidbodyConstraints groundedConstraints; // Stores rigidbody constraints for when grounded incase we need to change them in the air.
    private Vector3 mousePos; // Position of mouse cursor in world environment

    [Header("Setup")]
    [Tooltip("How fast the rat runs")]
    public float moveSpeed = 20f;
    [Tooltip("How HIGH the rat jumps")]
    public float jumpPower = 600f;
    [Tooltip("How FAR the rat jumps")]
    public float jumpForce = 16f;

    [Tooltip("If true, can freely rotate while jumping")]
    public bool canSpin = false;

    public enum jumpFreedom
    {
        Locked,
        SteerAllowed,
        SpeedControl,
        FreeMovement
    }

    [Tooltip("Controls how much freedom player has while jumping")]
    public jumpFreedom jumpStyle = jumpFreedom.Locked;

    [Header("Debug")]
    public bool moveState = true;
    public bool isJump = false;


    void Start()
    {
      rb = GetComponent<Rigidbody>(); // Get rat rigidbody
        groundedConstraints = rb.constraints;
    }

    // Update is called once per frame
    void Update()
    {
        
        mousePos = Input.mousePosition; //Get mouse position from input

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        //Get Rat position on screen through the camera
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        //Get the difference between the Mouse position and Rat position

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        //Get the angle to the mouse position using maths I don't fully understand (Reused code, its a prototype, im allowed)

       
        if (moveState || jumpStyle != jumpFreedom.Locked) //steer, speed and free can pass 
        {

            if (moveState || jumpStyle != jumpFreedom.SpeedControl) // steer and free can pass
            {

                transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
                //Aim Rat towards mouse pointer

            }

            if (moveState || jumpStyle != jumpFreedom.SteerAllowed) // speed and free can pass
            {

                if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.W))
                {
                    rb.AddForce(transform.right * moveSpeed);
                    //Accelerate Rat.

                    //  transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);

                    // ^ Unused, may be useful for finer control if we want Rat to go exactly to the mouse pointer
                }
                if (false) //(Input.GetKey(KeyCode.A))
                {
                    //Unused, allows Rat to strafe, is kind of disorienting
                    transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
                }
                if (false) //(Input.GetKey(KeyCode.D))
                {
                    //Unused, allows Rat to strafe, is kind of disorienting
                    transform.Translate(transform.forward * -moveSpeed * Time.deltaTime, Space.World);
                }
            }
        }
    
        if(Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Space)){ //JUMP INPUT

           moveState = false; //Player not grounded
           isJump = true; // Player is airborne (from a jump)


            if (!canSpin)
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezeRotationZ;

                rb.velocity = new Vector3(transform.right.x * jumpForce, jumpPower, transform.right.z * jumpForce);
            //Apply force to make the rat jump, Should feel fairly "set" so this is done once (unless we need to control it for steering)
        }

        //if Player is currently mid jump with jump steering allowed, allow them to change the rats direction still by holding the forward key.
        if (isJump && jumpStyle == jumpFreedom.SteerAllowed && (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.W)))
        {
            rb.velocity = new Vector3(transform.right.x * jumpForce, rb.velocity.y, transform.right.z * jumpForce);
            //Since this sets the XZ velocity to jumpForce, this might make the jump faster than the other settings, as the rigidbody likely slows that force down over the course of the jump, this resets it back to full speed.
        }

        //If Collision breaks, pressing X should force the player to re enter grounded state
        if(Input.GetKeyDown(KeyCode.X)){
            enterGrounded();
            }


        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed),rb.velocity.y,Mathf.Clamp(rb.velocity.z, -moveSpeed, moveSpeed));
        //Limits speed to the max of movespeed
    }

    void enterGrounded()
    {
        isJump = false;
        moveState = true;
        rb.constraints = groundedConstraints;
    }

    void OnCollisionEnter(Collision collision)
    {
        enterGrounded();
        //Enters grounded state on collision with anything

    }


}


/* Todo

Unlock rat rotation for easier ramp access (also makes the rat hop when flipped, explore this) (Rat resets rotation on landing, hopping is due to force carry over i think? nothing to actually change here) (DONE)
Lock rat rotation when jumping (make this an option, making the cube flip is funny) DONE
Create three different settings for jump controls (locked, steering allowed, free, etc) DONE
Investigate different force movement to allow different jump settings to have more options (air steering is currently useless) DONE

Try putting a placeholder model on
Setup some form of camera control
Give the rat a tail object?
Option for rat to auto slow down to stop exactly on mouse pointer, rather than always charging at it

*/