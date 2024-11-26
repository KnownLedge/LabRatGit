using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimbing_2 : MonoBehaviour
{
    [Header("Settings")]
    public float climbSpeed = 7f;
    [SerializeField] private float wallDetectionDistance = 0.3f;
    [SerializeField] private float wallSwitchDistance = 1f;
    [SerializeField] private float climbUpwardForce = 25f;
    [SerializeField] private float fromWallToGround = 15f;
    public LayerMask wallMask;

    private ConstantForce constantForce;
    private Rigidbody rb;
    private Ratmovement ratMovement;
    private StaminaController staminaController;

    [Header("Debug")]
    public bool isTouchingWall;
    public bool isClimbing;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ratMovement = GetComponent<Ratmovement>();
        constantForce = GetComponent<ConstantForce>();
        staminaController = GetComponent<StaminaController>();
    }

    void Update()
    {

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
            staminaController.Climbing();
            CheckForWallSwitch();
        }
    }

    void CheckWallContact()
    {
        // Perform Raycast directly from the rat's position to detect the wall
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallDetectionDistance, wallMask))
        {
            isTouchingWall = true;
        }
        else
        {
            isTouchingWall = false;
        }
    }


    void StartClimbing()
    {
        constantForce.enabled = false;
        isClimbing = true;
        ratMovement.isGrounded = false;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.drag = 0f;

        ratMovement.moveState = false;

        rb.AddForce(Vector3.up * climbUpwardForce, ForceMode.Impulse);

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, wallDetectionDistance, wallMask))
        {
            Vector3 wallNormal = hit.normal;
            transform.position = hit.point + wallNormal * 0.1f; // Adjust the offset as needed

            AlignRatToWall(wallNormal);
        } else {
            Debug.LogError("No wall detected");
        }
    }

    public void StopClimbing()
    {
        constantForce.enabled = true;
        isClimbing = false;
        ratMovement.isGrounded = true;
        rb.useGravity = true;
        rb.drag = 2f;

        ratMovement.moveState = true;

        // Restore constraints to prevent falling over
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Climb()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 climbDirection = (Vector3.up * vertical + right * horizontal) * climbSpeed;
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
            rb.AddForce(Vector3.up * fromWallToGround, ForceMode.Impulse);
        }
    }

    // Align the rat's rotation to be perpendicular to the wall
    private void AlignRatToWall(Vector3 wallNormal)
    {
        // Calculate the desired rotation perpendicular to the wall
        Quaternion targetRotation = Quaternion.LookRotation(-wallNormal, Vector3.up);

        // Instantly set the rotation if not yet aligned, or smooth it after first touch
        if (transform.rotation != targetRotation)
        {
            transform.rotation = targetRotation;  // Instant alignment
            StartCoroutine(SmoothAlignToWall(targetRotation));  // Smooth rotation afterward
        }
    }


    // Smoothly rotate the rat toward the target rotation
    IEnumerator SmoothAlignToWall(Quaternion targetRotation)
    {
        float duration = 0.2f; // Faster rotation (0.2 sec)
        float elapsedTime = 0f;
        Quaternion initialRotation = transform.rotation;

        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    void CheckForWallSwitch()
    {
        RaycastHit hit;
         if (Physics.Raycast(transform.position, transform.right, out hit, wallSwitchDistance, wallMask)) 
        {
            SwitchToWall(hit.normal);
        }
        // Detect adjacent wall on the left
        else if (Physics.Raycast(transform.position, -transform.right, out hit, wallSwitchDistance, wallMask))
        {
            SwitchToWall(hit.normal);
        }
    }

    void SwitchToWall(Vector3 newWallNormal)
    {
        // Align the rat to the new wall's surface
        transform.rotation = Quaternion.LookRotation(-newWallNormal, Vector3.up);
        rb.velocity = Vector3.zero; // Reset velocity for smooth transition
    }
}
