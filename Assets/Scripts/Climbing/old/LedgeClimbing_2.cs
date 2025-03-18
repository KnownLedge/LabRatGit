using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeClimbing_2 : MonoBehaviour
{
    [Header("Settings")]
    public float climbSpeed = 7f;
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private float ledgeJumpForce = 5f;
    [SerializeField] private LayerMask ledgeMask;
    private Vector3 targetLedgePosition;
    public bool isStickingToLedge = false;

    private Rigidbody rb;
    private Ratmovement ratMovement;
    private ConstantForce constantForce;
    private StaminaController staminaController;
    private ClimbTrigger climbTrigger;

    //Default value trackers
    private float defaultDrag;
    private RigidbodyConstraints defaultConstraints;

    [Header("Debug")]
    public bool isTouchingLedge;

    private Coroutine constantForceCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ratMovement = GetComponent<Ratmovement>();
        constantForce = GetComponent<ConstantForce>();
        staminaController = GetComponent<StaminaController>();
        climbTrigger = GetComponent<ClimbTrigger>();

        //Set current values to return to later
        defaultDrag = rb.drag;
        defaultConstraints = rb.constraints;

       // rb.constraints = RigidbodyConstraints.FreezeRotation;

        CheckLedgeContact();
        //Since the trigger script already checked if we can climb, we can climb straight away

    }

    void Update()
    {
        if(!ratMovement.isJump)
        {
            CheckLedgeContact();

            // Trigger climbing actions with E
            //if (Input.GetKeyDown(KeyCode.E))
            //{
                if (isTouchingLedge && !isStickingToLedge)
                {
                    StickToLedge();
                    staminaController.Climbing();
                } else if (isStickingToLedge && Input.GetKeyDown(KeyCode.E)) {
                   FallFromLedge();  // New function to handle falling off the ledge
                }
            //}

            // Drain stamina while sticking to ledge
            if (isStickingToLedge)
            {
                staminaController.Climbing();  // Drain stamina while stuck to the ledge
            }

            // Handle jumping off the ledge
            if (isStickingToLedge && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space pressed - attempting to jump off ledge.");
                JumpOffLedge();
            }

            // Handle air movement when not sticking to ledge
            if (!isStickingToLedge && !isTouchingLedge)
            {
                HandleAirMovement();

                // Adjust velocity to prevent excessive speed when not climbing
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, 10f); // Adjust the speed cap as needed
            }
        }   
    }

    // Check if the rat is close to a ledge for climbing
    void CheckLedgeContact()
    {
        isTouchingLedge = false;
        RaycastHit hit;

        // Check for ledge directly in front and above the rat
        Vector3 castStart = transform.position + Vector3.up * 1f; // Slightly offset above the rat

        // Perform SphereCast in front and upwards to detect ledge
        if (Physics.SphereCast(castStart, 0.5f, transform.forward, out hit, detectionRadius, ledgeMask))
        {
            isTouchingLedge = true;
            targetLedgePosition = hit.point;
            // Debugging ledge detection
            Debug.Log($"Ledge detected at position: {hit.point}");
        }
        else
        {
            // Check in an upward direction to ensure the ledge above is detected
            if (Physics.Raycast(castStart, Vector3.up, out hit, detectionRadius, ledgeMask))
            {
                isTouchingLedge = true;
                targetLedgePosition = hit.point;
                // Debugging ledge detection
                Debug.Log($"Ledge detected above at position: {hit.point}");
            }
        }
    }

    // Stick the rat to the ledge by freezing its movement and aligning its position
    void StickToLedge()
    {
        isStickingToLedge = true;
        constantForce.enabled = false; // Disable constant force
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Temporarily remove constraints for testing

        // Debugging the position before and after applying the offset
        Debug.Log("Current Position: " + transform.position);
        Debug.Log("Target Ledge Position: " + targetLedgePosition);

        // Apply an offset (try with -2 to move a noticeable distance back)
        Vector3 offset = transform.forward * -0.5f;  // Adjust this value if needed
        Vector3 targetPosition = targetLedgePosition + offset;

        // Log the new target position after offset
        Debug.Log("Target Position (after offset): " + targetPosition);

        // Move the rat to the target position
        transform.position = targetPosition;

        //Aim the rat at the ledge
        Vector3 relativePos = targetPosition - transform.position;
        transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.Rotate(new Vector3(0, 90, 0));//fix rat facing side on


        // Reset velocity, disable gravity, and apply drag
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.drag = 10f;

        Debug.Log("Rat stuck to ledge at position: " + transform.position);
    }

    // Jump off the ledge, re-enable gravity and remove freezing constraints
    void JumpOffLedge()
    {
        // Check if the player has enough stamina to jump
        if (staminaController.playerStamina >= staminaController.jumpCost)
        {
            // Drain stamina for jumping
            staminaController.playerStamina -= staminaController.jumpCost;
            staminaController.UpdateStamina(1);  // Update UI or any other logic for stamina

            // Perform the jump
            isStickingToLedge = false;
            rb.useGravity = true;  // Re-enable gravity

            // Remove constraints to allow free movement
          //  rb.constraints = RigidbodyConstraints.FreezeRotation;

            // Reset the vertical velocity to avoid odd behavior
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // Apply jump force to move the rat upwards
            rb.AddForce(Vector3.up * ledgeJumpForce, ForceMode.Impulse);

            // Set continuous collision detection mode to avoid clipping through walls
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            // Debug the velocity to monitor its behavior after applying the jump
            Debug.Log($"After applying jump force: {rb.velocity}");

            // Raycast to check for immediate collision after the jump
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f, ledgeMask))
            {
                // If we collide immediately after jumping, stop movement or adjust accordingly
                rb.velocity = Vector3.zero;
                Debug.Log("Collided with wall immediately after jump.");
            }

            Debug.Log("Jumped off ledge.");

            // Start the coroutine to enable constant force after delay
            if (constantForceCoroutine != null)
            {
                StopCoroutine(constantForceCoroutine); // Stop any ongoing coroutine
            }
            constantForceCoroutine = StartCoroutine(EnableConstantForceAfterDelay(1f)); // 2 seconds delay
        }
        else
        {
            Debug.Log("Not enough stamina to jump.");
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
         if(enabled){
            if(collision.gameObject.layer == 6) //6 Is the ground layer
            {
             resetRbVals();
                climbTrigger.triggerMovement();
            }
        }
    }

    void resetRbVals()
    {
        if(enabled){
        rb.useGravity = true;
        rb.drag = defaultDrag; // Reset drag to normal values
          //  rb.constraints = defaultConstraints;
        }
    }

    // Coroutine to enable constant force after a delay
    IEnumerator EnableConstantForceAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay time

        constantForce.enabled = true; // Enable constant force
        Debug.Log("Constant force enabled after delay.");
    }

    // Handle movement when in the air (e.g., using horizontal input)
    void HandleAirMovement()
    {
        //if (!ratMovement.moveState) // Only allow air movement if not grounded
        //{
            // Get horizontal input (A/D for left/right)
            float horizontal = Input.GetAxis("Horizontal"); 

            // Create the right direction relative to the rat's orientation
            Vector3 right = transform.right;

            // Vertical movement is automatic during climbing
            float vertical = 1f; // Constant vertical movement upwards (adjust this value to control climbing speed)

            // Calculate the climb direction based on horizontal and vertical input
            Vector3 climbDirection = (Vector3.up * vertical + right * horizontal) * climbSpeed;

            // Apply the new velocity to the Rigidbody
            rb.velocity = new Vector3(climbDirection.x, rb.velocity.y, climbDirection.z); // Maintain the current z velocity

            // Debugging output to confirm direction
            Debug.Log($"Climb Direction: {climbDirection} | New velocity: {rb.velocity}");
        //}
    }
    public void FallFromLedge()
    {
        // Stop sticking to the ledge
        isStickingToLedge = false;

        // Re-enable gravity and stop freezing movement
        resetRbVals();

        // Reset velocity to ensure the rat falls naturally
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // Remove any vertical velocity if needed

        // Log for debugging
        Debug.Log("Rat has fallen from the ledge.");

        climbTrigger.triggerMovement();

    }
}
