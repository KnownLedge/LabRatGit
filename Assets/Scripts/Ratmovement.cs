using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratmovement : MonoBehaviour
{
    private Rigidbody rb; // Player rigidbody component
    private RigidbodyConstraints groundedConstraints; // Stores rigidbody constraints for when grounded
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
    [Tooltip("How long after jumping before the Rat can reenter grounded state")]
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

    [Tooltip("Target Y rotation to maintain")]
    public float targetYRotation = 0f; // Adjust this as per your requirements

    [Tooltip("Rotation smoothing speed")]
    public float rotationSpeed = 10f; // For smoother adjustments

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get rat rigidbody
        groundedConstraints = rb.constraints;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Prevent clipping
    }

    void Update()
    {
        mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90f;

        if (moveState || jumpStyle != jumpFreedom.Locked)
        {
            AimRat(angle);
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

        // Only smooth the Y rotation when the rat is moving or rotating.
        if (moveState || jumpStyle != jumpFreedom.Locked)
        {
            FixYAxisRotation();
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
        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            enterGrounded();

            // Optionally clamp Y position to prevent sinking
            Vector3 position = transform.position;
            transform.position = new Vector3(position.x, Mathf.Max(position.y, 0.5f), position.z);

            rb.constraints = groundedConstraints;
        }
    }

    public void enterGrounded()
    {
        if (jumpLockOut < 0f)
        {
            isJump = false;
            moveState = true;

            // Reset Y-axis rotation to align forward
            Quaternion currentRotation = transform.rotation;
            transform.rotation = Quaternion.Euler(0, currentRotation.eulerAngles.y, 0);

            rb.constraints = groundedConstraints;
        }
    }

    public void AimRat(float angle)
    {
        if (moveState || jumpStyle != jumpFreedom.SpeedControl)
        {
            // Calculate target rotation
            Quaternion targetRotation = Quaternion.Euler(0, -angle, 0);

            // Calculate the difference in rotation
            Quaternion deltaRotation = targetRotation * Quaternion.Inverse(transform.rotation);
            deltaRotation.ToAngleAxis(out float angleDifference, out Vector3 rotationAxis);

            // Add a dead zone to ignore minor adjustments
            if (Mathf.Abs(angleDifference) > 1f) // Adjust this threshold as needed
            {
                // Normalize the axis and apply torque
                rotationAxis = transform.InverseTransformDirection(rotationAxis).normalized;
                rb.AddTorque(rotationAxis * angleDifference * turnPower * Time.deltaTime, ForceMode.VelocityChange);
            }
            else
            {
                // Apply damping to smooth rotation
                rb.angularVelocity *= 0.9f; // Adjust damping factor as needed
            }

            // Clamp angular velocity to prevent excessive twitching
            rb.angularVelocity = Vector3.ClampMagnitude(rb.angularVelocity, turnPower * 0.1f);
        }
    }

    public void MoveRat()
    {
        if (moveState || jumpStyle != jumpFreedom.SteerAllowed)
        {
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
        else
        {
            rb.velocity = Vector3.zero; // Prevent unintended movement
        }
    }

    public void JumpRat()
    {
        moveState = false;
        isJump = true;
        jumpLockOut = jumpLockOutTime;

        // Freeze Z-axis rotation during jump to prevent drifting
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;

        Vector3 forwardDirection = transform.forward;
        rb.velocity = new Vector3(forwardDirection.x * jumpForce, jumpPower, forwardDirection.z * jumpForce);

        rb.AddRelativeTorque(spinForce);
    }

    public void ChangeSpeed(int i)
    {
        if (speedStates[i] != null)
        {
            moveSpeed = speedStates[i].x;
            maxSpeed = speedStates[i].y;
        }
    }

    private void FixYAxisRotation()
    {
        // Get the current rotation
        Vector3 currentRotation = transform.eulerAngles;

        // Smoothly adjust the Y-axis only if the rat is moving
        if (Mathf.Abs(rb.velocity.x) > 0 || Mathf.Abs(rb.velocity.z) > 0)
        {
            float smoothYRotation = Mathf.LerpAngle(currentRotation.y, targetYRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(currentRotation.x, smoothYRotation, currentRotation.z);
        }
    }
}
