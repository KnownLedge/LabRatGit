using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 2f;               // Speed for moving to the ledge
    [SerializeField] private float detectionRadius = 1f;         // Radius to detect the ledge
    [SerializeField] private float directionTolerance = 0.5f;    // Lowered tolerance to allow ledges to the side
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

        if (Input.GetKeyDown(KeyCode.E) && !isClimbing)
        {
            if (isTouchingLedge)
            {
                StartClimbing();
            }
        }

        if (isClimbing)
        {
            if (isMovingToLedge)
            {
                MoveToLedge();
            }
            else
            {
                AlignRatToLedge();
                Climb();
            }
        }
    }

    void CheckLedgeContact()
    {
        isTouchingLedge = false;
        Collider[] ledgeColliders = Physics.OverlapSphere(transform.position, detectionRadius, ledgeMask);
        foreach (Collider ledge in ledgeColliders)
        {
            Vector3 toLedge = (ledge.transform.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(toLedge, transform.forward);

            // Debugging ledge detection
            Debug.Log($"Checking ledge at {ledge.transform.position} with dot Product: {dotProduct}");

            if (dotProduct >= directionTolerance)
            {
                isTouchingLedge = true;
                targetLedgePosition = ledge.transform.position;
                break;
            }
        }

        if (!isTouchingLedge)
        {
            Debug.LogWarning("No ledge found within detection radius.");
        }
    }

    void StartClimbing()
    {
        isClimbing = true;
        isMovingToLedge = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.drag = 10f;

        ratMovement.moveState = false;
        constantForce.enabled = false;

        Debug.Log("Starting climbing sequence.");
    }

    void StopClimbing()
    {
        isClimbing = false;
        isMovingToLedge = false;
        rb.useGravity = true;
        rb.drag = 2f;

        ratMovement.moveState = true;
        constantForce.enabled = true;

        Debug.Log("Stopping climbing sequence.");
    }

    void MoveToLedge()
    {
        // Move towards the ledge position
        if (Vector3.Distance(transform.position, targetLedgePosition) < 0.05f)
        {
            // If close enough, stop moving towards the ledge
            transform.position = targetLedgePosition;
            isMovingToLedge = false;

            // Freeze the movement and disable gravity to stick to the ledge
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            rb.drag = 10f;

            // Align the rat to the ledge surface
            AlignRatToLedge();
            Debug.Log("Rat stuck to ledge and aligned.");
        }
        else
        {
            // Continue moving towards the ledge if not reached yet
            transform.position = Vector3.MoveTowards(transform.position, targetLedgePosition, moveSpeed * Time.deltaTime);
            Debug.Log("Moving towards ledge at position: " + transform.position);
        }
    }

    private void AlignRatToLedge()
    {
        // Perform a raycast to check for the ledge normal and align the rat
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRadius, ledgeMask))
        {
            // Align the rat to the surface normal of the ledge
            Vector3 ledgeNormal = hit.normal;
            transform.rotation = Quaternion.LookRotation(ledgeNormal);
            Debug.Log("Aligned rat to ledge with normal: " + ledgeNormal);
        }
        else
        {
            Debug.LogError("No ledge detected while aligning.");
        }
    }

    // Additional climbing functionality when the rat is aligned with the ledge
    private void Climb()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 climbDirection = transform.forward * vertical + transform.right * horizontal;
        rb.velocity = climbDirection * moveSpeed;

        if (horizontal == 0f && vertical == 0f)
        {
            // When not pressing movement keys, freeze the velocity for stability
            rb.velocity = Vector3.zero;
        }
    }
}
