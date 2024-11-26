using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratmovement : MonoBehaviour
{
    private Rigidbody rb; //Player rigidbody component
    private RigidbodyConstraints groundedConstraints; // Stores rigidbody constraints for when grounded incase we need to change them in the air.
    private Vector3 mousePos; // Position of mouse cursor in world environment
    [SerializeField]private LayerMask groundLayer;

    [Header("Setup")]
    [Tooltip("How fast the rat runs")]
    public float moveSpeed = 20f;
    [Tooltip("Max speed the rat runs")]
    public float maxSpeed = 20f;
    [Tooltip("How HIGH the rat jumps")]
    public float jumpPower = 600f;
    [Tooltip("How fast the rat turns")]
    public float turnPower = 100f;
    [Tooltip("How FAR the rat jumps")]
    public float jumpForce = 16f;
    [Tooltip("How long after jumping before the Rat can reneter grounded state")]
    public float jumpLockOutTime = 0.3f;

    [Tooltip("How hard the rat spins, pure style points")]
    public Vector3 spinForce = new Vector3(0, 0, 0);

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
    [Tooltip("Iterated by number keys, sets movespeed and maxspeed for testing speed change")]
    public Vector2[] speedStates;

    [Header("Debug")]
    public bool moveState = true;
    public bool isJump = false;
    public float prevAngle = 0f;

    public float jumpLockOut = 0f;
    //How long before the player is allowed to land on an object when jumping, designed to prevent the player triggering ground state at the start of a jump.


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
        angle -= 90f;


        if (moveState || jumpStyle != jumpFreedom.Locked) //steer, speed and free can pass 
        {

            AimRat(angle);


        }


        if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Space)) && isJump == false)
        { //JUMP INPUT

            JumpRat();

        }

        //if Player is currently mid jump with jump steering allowed, allow them to change the rats direction still by holding the forward key.
        if (isJump && jumpStyle == jumpFreedom.SteerAllowed && (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.W)))
        {
            rb.velocity = new Vector3(transform.right.x * jumpForce, rb.velocity.y, transform.right.z * jumpForce);
            //Since this sets the XZ velocity to jumpForce, this might make the jump faster than the other settings, as the rigidbody likely slows that force down over the course of the jump, this resets it back to full speed.
        }

        //If Collision breaks, pressing X should force the player to re enter grounded state
        if (Input.GetKeyDown(KeyCode.X))
        {
            enterGrounded();
        }
        jumpLockOut -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeSpeed(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeSpeed(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            changeSpeed(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            changeSpeed(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            changeSpeed(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            changeSpeed(4);
        }


    }

    public void enterGrounded()
    {
        if (jumpLockOut < 0f)
        {
            isJump = false;
            moveState = true;
            rb.constraints = groundedConstraints;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is on the ground layer
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            enterGrounded(); // Call your method to set the grounded state

            // Freeze rotation so the rat stays on its feet
            rb.constraints = RigidbodyConstraints.FreezeRotationZ;

            // Align the rat's rotation with the ground's normal (to keep the rat upright)
            Vector3 groundNormal = collision.contacts[0].normal;
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Smooth the rotation
        }
        else
        {
            // If it's not the ground, you may want to unfreeze rotation if necessary
            rb.constraints = RigidbodyConstraints.None; // Allow rotation
        }
    }

    void FixedUpdate()
    {
        if (moveState || jumpStyle != jumpFreedom.Locked) //steer, speed and free can pass 
        {


            MoveRat();
        }
    }

    public void AimRat(float angle)
    {
        if (moveState || jumpStyle != jumpFreedom.SpeedControl) // steer and free can pass
        {
            // Calculate the target rotation in the Y-axis direction
            Quaternion targetRotation = Quaternion.Euler(0, -angle, 0);
            
            // Get the current rotation and the difference
            Quaternion currentRotation = transform.rotation;
            Quaternion rotationDifference = targetRotation * Quaternion.Inverse(currentRotation);

            // Extract the yaw (rotation around the Y-axis) from the difference
            float yaw = rotationDifference.eulerAngles.y;

            // Normalize yaw to avoid weird behavior when crossing 180 degrees
            if (yaw > 180f) yaw -= 360f;

            // Apply torque to rotate the rat towards the mouse direction
            Vector3 torque = Vector3.up * yaw * turnPower * Time.deltaTime;

            // Apply torque using Rigidbody's AddTorque to smoothly rotate towards the mouse
            rb.AddTorque(torque, ForceMode.Force);
        }
    }

    public void MoveRat()
    {
        if (moveState || jumpStyle != jumpFreedom.SteerAllowed) // speed and free can pass
        {

            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.W))
            {
                rb.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);
            }
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed));
        }
    }

    public void JumpRat()
    {
        moveState = false; // Player not grounded
        isJump = true; // Player is airborne (from a jump)
        jumpLockOut = jumpLockOutTime;

        // Prevent spinning during the jump
        if (!canSpin)
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezeRotationZ;

        // Get the forward direction based on the rat's current rotation
        Vector3 forwardDirection = transform.forward;

        // Set the jump velocity with both vertical and forward components
        rb.velocity = new Vector3(forwardDirection.x * jumpForce, jumpPower, forwardDirection.z * jumpForce); 

        // Apply torque for spin (if needed)
        rb.AddRelativeTorque(spinForce); // Adjust spin force as needed
    }


    void OnCollisionExit(Collision collision)
    {
        // Check if the object leaving is part of the ground layer
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Unfreeze rotation when leaving the ground
            rb.constraints = RigidbodyConstraints.None; // Allow rotation when leaving ground
        }
    }



    public void changeSpeed(int i)
    {
        if (speedStates[i] != null)
        {
            moveSpeed = speedStates[i].x;
            maxSpeed = speedStates[i].y;
        }
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