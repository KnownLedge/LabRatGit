using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratmovement : MonoBehaviour
{
    protected Rigidbody rb; //Player rigidbody component
    protected RigidbodyConstraints groundedConstraints; // Stores rigidbody constraints for when grounded incase we need to change them in the air.
    protected Vector3 mousePos; // Position of mouse cursor in world environment
    [SerializeField] protected LayerMask groundLayer;

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
    [Tooltip("Stops rat drifting by dividing by this value")]
    public float driftStop = 1;

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

    [Header("Body")]
    [SerializeField] protected Transform frontLeg;
    [SerializeField] public Transform backLeg; //Changed to public to make killbox script easier, we can set back to private later
    [SerializeField] protected Rigidbody backRB; 
    [SerializeField] protected float groundCheckDistance = 0.2f;


    [SerializeField] protected float backMoveSpeed = 5f;
    [SerializeField] protected float backJumpForce = 16f;
    [SerializeField] protected float backJumpPower = 100f;
    [SerializeField] protected float backJumpDelay = 0.1f;
    [SerializeField] protected float frontJumpDelay = 0.1f;

    [Header("Balance")]
    [SerializeField] protected float balanceForceMultiplier = 10f;
    [SerializeField] protected float tiltThreshold = 60f;
    [SerializeField] protected float correctiveSpeed = 2f;

    [Header("Debug")]
    [SerializeField] protected bool isFrontGrounded = false;
    [SerializeField] protected bool isBackGrounded = false;
    public bool moveState = true;
    public bool isJump = false; //Keep public, needed for animator
    public float prevAngle = 0f;
    public float jumpLockOut = 0f;
    //How long before the player is allowed to land on an object when jumping, designed to prevent the player 
    //triggering ground state at the start of a jump.

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        backRB = backLeg.GetComponent<Rigidbody>();
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

        CheckGroundedState();
    }

    void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero; // Prevent unwanted rotation

        if (moveState || jumpStyle != jumpFreedom.Locked)
        {
        //Input taken outside of function to simplify making ai character controller
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.W))
            {
                MoveRat();
            }else{
                backRB.velocity = new Vector3(0, backRB.velocity.y, 0);
            }
        }
        BalanceRat();
    }

    public void MoveRat()
    {
        // Prevent movement when climbing or in an invalid state
        if (moveState || jumpStyle != jumpFreedom.SteerAllowed)
        {
            // Normal movement logic for the rat when it's not climbing

                rb.AddForce(new Vector3(transform.forward.x, 0, transform.forward.z) * moveSpeed, ForceMode.Impulse);

                //Going to apply force to back of rat to aim it towards the center of the rat, should make the body move more controlled of itself
                Vector3 middleDir = (transform.position - backLeg.position) / 2;
                backRB.AddForce(new Vector3(middleDir.x,0, middleDir.z) *backMoveSpeed, ForceMode.Impulse);
          //  }
           // else
           // {
                rb.velocity = new Vector3(rb.velocity.x / driftStop, rb.velocity.y, rb.velocity.z / driftStop);
                backRB.velocity = new Vector3(backRB.velocity.x / driftStop, backRB.velocity.y, backRB.velocity.z / driftStop);

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
            // Calculate the target rotation in the Y-axis direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            // Get the current rotation and the difference
            Quaternion currentRotation = transform.rotation;
            Quaternion rotationDifference = targetRotation * Quaternion.Inverse(currentRotation);
            
            // Extract the yaw (rotation around the Y-axis) from the difference
            float yaw = rotationDifference.eulerAngles.y;
            
            // Normalize yaw to avoid weird behavior when crossing 180 degrees
            if (yaw > 180f) yaw -= 360f;

            if (Mathf.Abs(yaw) > 0.1f)
            {
                Vector3 torque = Vector3.up * yaw * turnPower * Time.deltaTime;
                rb.AddTorque(torque, ForceMode.Force);
            }
        }

 
        


    }

    public void JumpRat()
    {
        if(!CanJump()) return;

        moveState = false;
        isJump = true;
        jumpLockOut = jumpLockOutTime;

        Vector3 forwardDirection = transform.forward;

        if((isFrontGrounded && !isBackGrounded) || (isFrontGrounded && isBackGrounded))
        {
            rb.velocity = new Vector3(forwardDirection.x * jumpForce, jumpPower, forwardDirection.z * jumpForce);
            StartCoroutine(DelayedBackLegJump(backJumpDelay));
        } else if(isBackGrounded && !isFrontGrounded) {
            backRB.velocity = new Vector3(forwardDirection.x * backJumpForce, backJumpPower, forwardDirection.z * backJumpForce);
            StartCoroutine(DelayedFrontLegJump(frontJumpDelay));
        }

        rb.AddRelativeTorque(spinForce);
    }

    protected void BalanceRat()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * correctiveSpeed);

        float tiltAngle = Vector3.Angle(transform.up, Vector3.up);
        if (tiltAngle > tiltThreshold)
        {
            Vector3 correctiveTorque = Vector3.Cross(transform.up, Vector3.up) * balanceForceMultiplier;
            rb.AddTorque(correctiveTorque, ForceMode.Acceleration);
        }


    }

    public void ChangeSpeed(int i)
    {
        if (speedStates[i] != null)
        {
            moveSpeed = speedStates[i].x;
            maxSpeed = speedStates[i].y;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            CheckGroundedState();

            if(isFrontGrounded && isBackGrounded)
            {
                isJump = false;
            }

            var normal = collision.contacts[0].normal;
            if (normal.y > 0) 
            { //If colliding with the bottom of the rat
                enterGrounded();
         //       rb.constraints = groundedConstraints;
            }
        }
    }

    //void OnCollisionExit(Collision collision)
    //{
    //    // Check if the object leaving is part of the ground layer
    //    if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
    //    {
    //        // Unfreeze rotation when leaving the ground
    //    //    rb.constraints = RigidbodyConstraints.None; // Allow rotation when leaving ground
    //    }
    //}

    public void CheckGroundedState() //Set public for inheritence
    {
        RaycastHit sphereRay;
        isFrontGrounded = Physics.SphereCast(frontLeg.position, frontLeg.localScale.y, Vector3.down,out sphereRay, groundCheckDistance, groundLayer, QueryTriggerInteraction.UseGlobal);
        isBackGrounded = Physics.SphereCast(backLeg.position, backLeg.localScale.y, Vector3.down, out sphereRay, groundCheckDistance, groundLayer, QueryTriggerInteraction.UseGlobal);
        
        //   isBackGrounded = Physics.Raycast(backLeg.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    protected bool CanJump()
    {
        return (isFrontGrounded || isBackGrounded) && !isJump && jumpLockOut < 0f;
    }

    protected IEnumerator DelayedFrontLegJump(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(!isFrontGrounded) yield break;

        Vector3 forwardDirection = transform.forward;
        rb.velocity = new Vector3(forwardDirection.x * backJumpForce, jumpPower, forwardDirection.z * jumpForce);
    }

    protected IEnumerator DelayedBackLegJump(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(!isBackGrounded) yield break;

        Vector3 forwardDirection = transform.forward;
        backRB.velocity = new Vector3(forwardDirection.x * backJumpForce, backJumpPower, forwardDirection.z * backJumpForce);
    }

    public void enterGrounded()
    {
        if (jumpLockOut < 0f)
        {
            isJump = false;
            moveState = true;
            // rb.constraints = groundedConstraints;
		    // transform.rotation = new Quaternion();
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