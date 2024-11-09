using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbing : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float climbSpeed = 2f;
    [SerializeField] private float wallDetectionDistance = 2f;
    [SerializeField] private float climbUpwardForce = 2f;
    public LayerMask groundMask;
    public LayerMask wallMask;


    private Rigidbody rb;
    private Ratmovement ratMovement;

    [Header("Debug")]
    public bool isTouchingWall;
    public bool isClimbing;
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ratMovement = GetComponent<Ratmovement>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, 0.05f, groundMask);

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
        isTouchingWall = Physics.CheckSphere(transform.position, 0.5f, wallMask);

        if (!isTouchingWall)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.right, out hit, wallDetectionDistance, wallMask))
            {
                isTouchingWall = true;
            }
        }
    }

    void StartClimbing()
    {
        Debug.Log("StartClimbing called");
        isClimbing = true;
        isGrounded = false;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.drag = 0f;

        ratMovement.moveState = false;
        Debug.Log($"Rat Movement State: {ratMovement.moveState}");

        rb.AddForce(Vector3.up * climbUpwardForce, ForceMode.Impulse);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallDetectionDistance, wallMask))
        {
            Vector3 wallNormal = hit.normal;
            Quaternion targetRotation = Quaternion.LookRotation(-wallNormal, Vector3.up) * Quaternion.Euler(90f, 0f, 0f);
            
            Debug.Log($"Current Rotation: {transform.rotation.eulerAngles}, Target Rotation: {targetRotation.eulerAngles}");
            transform.rotation = targetRotation;
        }
    }

    void StopClimbing()
    {
        Debug.Log("StopClimbing called");
        isClimbing = false;
        isGrounded = true;
        rb.useGravity = true;
        rb.drag = 2f;

        ratMovement.moveState = true;
    }

    public void Climb()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 climbDirection = (forward * vertical + right * horizontal) * climbSpeed;
        rb.velocity = climbDirection;

        if (!isTouchingWall)
        {
            StopClimbing();
        }
    }
}
