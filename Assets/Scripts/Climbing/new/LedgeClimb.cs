using System.Collections;
using UnityEngine;

public class LedgeClimb : MonoBehaviour
{
    public float ledgeDetectDistance = 1f; // Forward distance for raycast to check for a ledge
    public float climbUpHeight = 2.0f; // Height to move the player up during a climb
    public float climbForwardDistance = 1.0f; // Distance to move the player forward onto the ledge
    public float climbDuration = 0.5f; // Duration of the climbing animation
    private bool hasTouchedGround = false; // Ensures the player has touched the ground before climbing again
    private bool isClimbing = false; // Check for if the player is currently climbing
    private Rigidbody rb;
    private Ratmovement ratMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        ratMovement = GetComponent<Ratmovement>(); // Get the movement script
    }

    void Update()
    {
        // Check if the player is on the ground
        if (Physics.Raycast(transform.position, Vector3.down, 1.1f))
        {
            hasTouchedGround = true;
        }

        // Attempt a ledge climb if not already climbing and the player has touched the ground
        if (!isClimbing && hasTouchedGround)
        {
            AttemptLedgeClimb();
        }
    }

    void AttemptLedgeClimb()
    {
        // Set the start position for ledge detection raycast
        Vector3 startRay = transform.position;

        // Set the ray direction
        Vector3 forwardRay = transform.forward * ledgeDetectDistance;

        // Cast a ray forward to detect a ledge
        if (Physics.Raycast(startRay, forwardRay, out RaycastHit hit, ledgeDetectDistance))
        {
            // Check if the hit object has the right tag
            if (hit.collider.CompareTag("Ledge"))
            {
                Vector3 ledgePosition = hit.point; // Get the ledge position
                StartCoroutine(ClimbLedge(ledgePosition)); // Start climbing
            }
        }
    }


    IEnumerator ClimbLedge(Vector3 ledgePosition)
    {
        isClimbing = true; 
        ratMovement.enabled = false; // Disable movement during climb
        rb.isKinematic = true; // Disable physics during climb

        // Define start position and target position for the climb
        Vector3 startPosition = transform.position;
        Vector3 climbTarget = new Vector3(ledgePosition.x, ledgePosition.y + climbUpHeight, ledgePosition.z) + transform.forward * climbForwardDistance;

        float elapsedTime = 0f;

        // Smoothly rotates the rat over the climb duration
        while (elapsedTime < climbDuration)
        {
            transform.position = Vector3.Lerp(startPosition, climbTarget, elapsedTime / climbDuration);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(-60, transform.rotation.eulerAngles.y, 0), elapsedTime / climbDuration / 2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position and reset state after climbing
        transform.position = climbTarget;
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        rb.isKinematic = false; // Re-enable physics
        ratMovement.enabled = true; // Re-enable movement
        isClimbing = false; // Reset climbing state
        hasTouchedGround = false; // Prevent repeated climbing without touching the ground
    }
}
