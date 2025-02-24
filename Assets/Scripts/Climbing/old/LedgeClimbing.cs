using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LedgeClimbing : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 2f;               // Speed for moving to the ledge
    [SerializeField] private float detectionRadius = 2f;         // Radius to detect the ledge
    [SerializeField] private float directionTolerance = 0.5f;    // Tolerance for ledge detection
    [SerializeField] private LayerMask ledgeMask;                // Layer mask for detecting ledges
    private Vector3 targetLedgePosition;                         // The target position to move to
    private bool isMovingToLedge = false;                        // Whether the rat is moving towards the ledge

    private ConstantForce constantForce;
    private Rigidbody rb;
    private Ratmovement ratMovement;

    [Header("Debug")]
    public bool isTouchingLedge;
    public bool isClimbing;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ratMovement = GetComponent<Ratmovement>();
        constantForce = GetComponent<ConstantForce>();
    }

    void Update()
    {
        CheckLedgeContact();

        // Handle climbing activation and deactivation with the E key
        if (Input.GetKeyDown(KeyCode.E) && !isClimbing)
        {
            if (isTouchingLedge)
            {
                StartClimbing();
            }
        } 
        else if (Input.GetKeyDown(KeyCode.E) && isClimbing)
        {
            StopClimbing();
        }

        if (isClimbing)
        {
            if (isMovingToLedge)
            {
                MoveToLedge();
            }
            else
            {
                HandleClimbingInput();
            }
        }
    }

    // Check for ledge contact using SphereCast
    void CheckLedgeContact()
    {
        isTouchingLedge = false;
        RaycastHit hit;

        // Check for ledge directly in front and above the rat
        Vector3 castStart = transform.position + Vector3.up * 0.5f; // Slightly offset above the rat

        // Perform SphereCast in front and upwards to detect ledge
        if (Physics.SphereCast(castStart, 0.5f, transform.forward, out hit, detectionRadius, ledgeMask))
        {
            isTouchingLedge = true;
            targetLedgePosition = hit.point;
            Debug.Log($"Ledge detected at position: {hit.point}");
        }
        else
        {
            // Check in an upward direction to ensure the ledge above is detected
            if (Physics.Raycast(castStart, Vector3.up, out hit, detectionRadius, ledgeMask))
            {
                isTouchingLedge = true;
                targetLedgePosition = hit.point;
                Debug.Log($"Ledge detected above at position: {hit.point}");
            }
        }

        // Visualize the SphereCast and Raycast for debugging purposes
        Debug.DrawRay(castStart, transform.forward * detectionRadius, Color.red, 0.5f);
        Debug.DrawRay(castStart, Vector3.up * detectionRadius, Color.green, 0.5f);
    }

    // Start the climbing sequence
    void StartClimbing()
    {
        isClimbing = true;
        isMovingToLedge = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.drag = 10f;

        ratMovement.moveState = false;
        constantForce.enabled = false;

        // Freeze rotation completely during climbing
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Disable rotation during climbing
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        Debug.Log("Starting climbing sequence.");
    }

    // Stop the climbing sequence
    void StopClimbing()
    {
        isClimbing = false;
        isMovingToLedge = false;
        rb.useGravity = true;
        rb.drag = 2f;

        ratMovement.moveState = true;
        constantForce.enabled = true;

        // Reset the Rigidbody constraints to allow full movement again
        rb.constraints = RigidbodyConstraints.None;

        Debug.Log("Stopping climbing sequence.");
    }

    // Move the rat to the ledge position
    void MoveToLedge()
    {
        if (Vector3.Distance(transform.position, targetLedgePosition) < 0.05f)
        {
            // Position the rat slightly before the ledge to avoid clipping
            Vector3 offset = transform.forward * -0.2f; // Adjust the offset value as needed
            transform.position = targetLedgePosition + offset;

            // Make sure the rat is not stuck inside the wall by adjusting position
            Vector3 directionToLedge = targetLedgePosition - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToLedge.normalized, out hit, directionToLedge.magnitude, ledgeMask))
            {
                transform.position = hit.point;
            }

            isMovingToLedge = false;

            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            rb.drag = 10f;

            Debug.Log("Rat stuck to ledge and aligned.");
        }
        else
        {
            // Immediately move the rat to the ledge without smooth transition
            transform.position = targetLedgePosition;
            isMovingToLedge = false;

            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            rb.drag = 10f;

            Debug.Log("Rat instantly moved to ledge at position: " + transform.position);
        }
    }

    // Handle climbing input (moving around the ledge)
    private void HandleClimbingInput()
    {
        if (Input.GetKey(KeyCode.W)) // Move upward (Jump to higher ledge)
        {
            TryMoveToLedge(Vector3.up); // Moves upward (Y-axis)
        }
        else if (Input.GetKey(KeyCode.S)) // Move downward (Drop to lower ledge)
        {
            TryMoveToLedge(Vector3.down); // Moves downward (Y-axis)
        }
        else if (Input.GetKey(KeyCode.A)) // Move left (Move to left ledge)
        {
            // Convert local left to world space
            Vector3 localLeft = transform.TransformDirection(Vector3.left);
            TryMoveToLedge(localLeft); // Move left along local X-axis
        }
        else if (Input.GetKey(KeyCode.D)) // Move right (Move to right ledge)
        {
            // Convert local right to world space
            Vector3 localRight = transform.TransformDirection(Vector3.right);
            TryMoveToLedge(localRight); // Move right along local X-axis
        }
    }


    private void TryMoveToLedge(Vector3 direction)
    {
        RaycastHit hit;

        // Define a starting point slightly in front of the rat to prevent direct collisions
        Vector3 castStart = transform.position + direction * 0.5f; // Adjust the offset as needed

        // Visualize the SphereCast start position with a sphere using Gizmos
        Debug.DrawRay(castStart, direction * detectionRadius, Color.green, 0.1f); // Draw a ray to show the cast direction

        // Perform a SphereCast in the given direction to detect ledges
        if (Physics.SphereCast(castStart, 0.5f, direction, out hit, detectionRadius, ledgeMask))
        {
            // If a ledge is detected, move the rat to the detected ledge
            targetLedgePosition = hit.point;

            // Move the rat immediately to the ledge without smooth transition
            transform.position = targetLedgePosition;

            // Optionally, add any logic you need after moving to the ledge, like stopping movement or freezing position
            isMovingToLedge = false;

            // Optionally, log the result for debugging
            Debug.Log("Detected ledge at: " + targetLedgePosition);

            // Visualize the SphereCast hit position
            Debug.DrawRay(hit.point, Vector3.up * 0.2f, Color.red, 0.1f); // Draw a ray from the hit position to visualize the ledge found
        }
        else
        {
            // If no ledge is found in the given direction, log the result
            Debug.Log("No ledge detected in direction: " + direction);
        }
    }

    private void OnDrawGizmos()
    {
        // Check if the object is selected and visualize the SphereCast detection radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f); // The sphere's radius for visualization
    }

}
