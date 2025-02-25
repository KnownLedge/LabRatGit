using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratmovement : MonoBehaviour
{
    private Rigidbody rb; //Player rigidbody component
    private RigidbodyConstraints groundedConstraints; // Stores rigidbody constraints for when grounded incase we need to change them in the air.
    private Vector3 mousePos; // Position of mouse cursor in world environment
    [SerializeField] private LayerMask groundLayer;

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
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        groundedConstraints = rb.constraints;
    }

    void Update()
    {
        mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        if (moveState || jumpStyle != jumpFreedom.Locked)
        {
            AimRat();
        }

        if ((Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Space)) && !isJump)
        {
            JumpRat();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            enterGrounded();
        }

        jumpLockOut -= Time.deltaTime;

        for (int i = 0; i < speedStates.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ChangeSpeed(i);
            }
        }

    }

    void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero; // Prevent unwanted rotation

        if (moveState || jumpStyle != jumpFreedom.Locked)
        {
            MoveRat();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        // if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        //  {
        var normal = collision.contacts[0].normal;
        if (normal.y > 0) { //If colliding with the bottom of the rat
            enterGrounded();
            rb.constraints = groundedConstraints;
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

    public void AimRat()
    {
        if (Camera.main == null)
            return;

        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = playerScreenPos.z;

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        Vector3 direction = worldMousePos - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnPower * Time.deltaTime);
        }
    }



    public void MoveRat()
    {
        // Prevent movement when climbing or in an invalid state
        if (moveState || jumpStyle != jumpFreedom.SteerAllowed)
        {
            // Normal movement logic for the rat when it's not climbing
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.W))
            {
                rb.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);
            }

            // Ensure the rat's velocity is capped at the max speed
            rb.velocity = new Vector3(
            Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed),
            rb.velocity.y,
            Mathf.Clamp(rb.velocity.z, -maxSpeed, maxSpeed)
        );
        }
        // If you are in climbing state or invalid state, stop moving
        else
        {
            rb.velocity = Vector3.zero;  // Prevent unintended movement
        }
    }



    public void JumpRat()
    {
        moveState = false;
        isJump = true;
        jumpLockOut = jumpLockOutTime;

        if (!canSpin)
            rb.constraints = rb.constraints | RigidbodyConstraints.FreezeRotationZ;

        Vector3 forwardDirection = transform.forward;
        rb.velocity = new Vector3(forwardDirection.x * jumpForce, jumpPower, forwardDirection.z * jumpForce);

        rb.AddRelativeTorque(spinForce);
    }

    //void OnCollisionExit(Collision collision)
    //{
    //    // Check if the object leaving is part of the ground layer
    //    if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
    //    {
    //        // Unfreeze rotation when leaving the ground
    //        rb.constraints = RigidbodyConstraints.None; // Allow rotation when leaving ground
    //    }
    //}

    public void ChangeSpeed(int i)
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