using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbing : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private float wallDetectionDistance = 1f;
    [SerializeField] private float climbUpwardForce = 150f;
    public LayerMask groundMask;
    public LayerMask wallMask;


    private ConstantForce constantForce;
    private Rigidbody rb;
    private Ratmovement ratMovement;
    private Quaternion originalRotation;  // To store the original rotation

    [Header("Debug")]
    public bool isTouchingWall;
    public bool isClimbing;
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ratMovement = GetComponent<Ratmovement>();
        constantForce = GetComponent<ConstantForce>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, 1f, groundMask);

        CheckWallContact();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isClimbing)
            {
                StopClimbing();
            }
            else if (isTouchingWall && !isClimbing)
            {
                StartClimbing();
            }
        }

        if (isClimbing)
        {
            Climb();
        }
    }

    void CheckWallContact()
    {
        isTouchingWall = Physics.CheckSphere(transform.position + transform.forward * wallDetectionDistance, 0.5f, wallMask);

        if (!isTouchingWall)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, wallDetectionDistance, wallMask))
            {
                isTouchingWall = true;
            }
        }
    }

    void StartClimbing()
    {
        constantForce.enabled = false;
        isClimbing = true;
        isGrounded = false;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.drag = 0f;

        ratMovement.moveState = false;

        rb.AddForce(Vector3.up * climbUpwardForce, ForceMode.Impulse);

        // Allow full rotation
        rb.constraints = RigidbodyConstraints.None;
        originalRotation = transform.rotation;

        float adjustedYAngle = originalRotation.eulerAngles.y > 180 ? originalRotation.eulerAngles.y - 360 : originalRotation.eulerAngles.y;
        Debug.Log("Original rotation y: " + adjustedYAngle);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallDetectionDistance, wallMask))
        {
            Vector3 wallNormal = hit.normal;
            transform.position = hit.point + wallNormal * 0.1f; // Adjust the offset as needed
            transform.rotation = Quaternion.Euler(-90, adjustedYAngle, 0);  
        } else {
            Debug.LogError("No wall detected");
        }
    }

    void StopClimbing()
    {
        constantForce.enabled = true;
        isClimbing = false;
        isGrounded = true;
        rb.useGravity = true;
        rb.drag = 2f;

        ratMovement.moveState = true;

        // Restore constraints to prevent falling over
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
    }

    public void Climb()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 climbDirection = (forward * vertical + right * horizontal) * climbSpeed;
        rb.velocity = climbDirection;

        // Manually adjust position to keep the rat attached to the wall
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.forward, out hit, wallDetectionDistance, wallMask))
        {
            transform.position = hit.point + hit.normal * 0.1f; // Adjust the offset as needed
        }

        if (!isTouchingWall)
        {
            StopClimbing();
        }
    }
}
