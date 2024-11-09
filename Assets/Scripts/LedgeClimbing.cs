using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float ledgeClimbUpForce = 2f;  // Force for climbing up the ledge
    [SerializeField] private float ledgeDetectionDistance = 0.5f;  // How far to detect a ledge
    [SerializeField] private LayerMask ledgeMask; 

    [Header("Debug")]
    [SerializeField]private bool isGrabbingLedge;

    
    private int jumpToLedge = 0;
    private Vector3 ledgePosition;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckAndStickToLedge();

        if (isGrabbingLedge && jumpToLedge != 0)
        {
            StickToLedge();
        }
    }

    // Check if the rat is near a ledge
    private void CheckAndStickToLedge()
    {
        if (!isGrabbingLedge)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, ledgeDetectionDistance, ledgeMask))
            {
                if (Input.GetKeyDown("W")) { jumpToLedge = 1; }
                else if (Input.GetKeyDown("S")) { jumpToLedge = 2; }
                else if (Input.GetKeyDown("D")) { jumpToLedge = 3; }
                else if (Input.GetKeyDown("A")) { jumpToLedge = 4; }
                isGrabbingLedge = true;
            }
        }
    }

    // Method to stick the rat to the ledge and disable movement
    private void StickToLedge()
    {
        RaycastHit hit;
        if (isGrabbingLedge)
        {
            rb.velocity = Vector3.zero;  // Stop the rat's current movement
            rb.useGravity = false;       // Disable gravity while on ledge

            switch (jumpToLedge)
            {
                case 1:
                    Physics.Raycast(transform.position, transform.forward, out hit, ledgeDetectionDistance, ledgeMask);
                    // If a ledge is detected
                    ledgePosition = hit.point; // Store the ledge position
                    transform.position = ledgePosition;  // Move the rat to the ledge position = ledgePosition;  // Move the rat to the ledge position
                    break;
                case 2:
                    Physics.Raycast(transform.position, -transform.forward, out hit, ledgeDetectionDistance, ledgeMask);
                    // If a ledge is detected
                    ledgePosition = hit.point; // Store the ledge position
                    transform.position = ledgePosition;  // Move the rat to the ledge position
                    break;
                case 3:
                    Physics.Raycast(transform.position, transform.right, out hit, ledgeDetectionDistance, ledgeMask);
                    // If a ledge is detected
                    ledgePosition = hit.point; // Store the ledge position
                    transform.position = ledgePosition;  // Move the rat to the ledge position
                    break;
                case 4:
                    Physics.Raycast(transform.position, -transform.right, out hit, ledgeDetectionDistance, ledgeMask);
                    // If a ledge is detected
                    ledgePosition = hit.point; // Store the ledge position
                    transform.position = ledgePosition;  // Move the rat to the ledge position
                    break;
            }
        }
    }
}
