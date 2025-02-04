using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeTransport : MonoBehaviour
{
    [SerializeField] private Transform entryA;
    [SerializeField] private Transform entryB;
    [SerializeField] private Collider triggerA;
    [SerializeField] private Collider triggerB;
    [SerializeField] private float travelSpeed = 10f;
    [SerializeField] private float exitForce = 5f;
    [SerializeField] private float cooldownTime = 1.5f;

    [SerializeField] private List<Transform> waypointsA; // Waypoints for entryA → exitB
    [SerializeField] private List<Transform> waypointsB; // Waypoints for entryB → exitA
    [SerializeField] private Ratmovement ratmovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform entryPoint = GetClosestEntry(other.transform.position);
            Transform exitPoint = (entryPoint == entryA) ? entryB : entryA;
            Collider entryTrigger = (entryPoint == entryA) ? triggerA : triggerB;
            Collider exitTrigger = (entryPoint == entryA) ? triggerB : triggerA;
            ratmovement.enabled = false;
            StartCoroutine(TransportPlayer(other.gameObject, entryPoint, exitPoint, entryTrigger, exitTrigger));
        }
    }

    private Transform GetClosestEntry(Vector3 playerPosition)
    {
        return (Vector3.Distance(playerPosition, entryA.position) < Vector3.Distance(playerPosition, entryB.position)) ? entryA : entryB;
    }

    private IEnumerator TransportPlayer(GameObject player, Transform entry, Transform exit, Collider entryTrigger, Collider exitTrigger)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb == null) yield break;

        // Disable colliders
        entryTrigger.enabled = false;
        exitTrigger.enabled = false;

        // Determine waypoints inside the function
        List<Transform> waypoints = (entry == entryA) ? waypointsA : waypointsB;

        // Cache Transform for performance
        Transform playerTransform = player.transform;

        rb.isKinematic = true;
        rb.freezeRotation = true;
        playerTransform.position = entry.position;

        // Make the rat look towards the first waypoint or exit
        Vector3 lookTarget = (waypoints != null && waypoints.Count > 0) ? waypoints[0].position : exit.position;
        playerTransform.rotation = Quaternion.LookRotation(lookTarget - playerTransform.position);
        playerTransform.rotation = Quaternion.Euler(0f, playerTransform.eulerAngles.y, 0f); // Reset X and Z rotation

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogWarning("No waypoints assigned! Moving directly to the exit.");
        }
        else
        {
            // Move smoothly through waypoints
            foreach (Transform target in waypoints)
            {
                Vector3 targetPosition = target.position;
                while (Vector3.Distance(playerTransform.position, targetPosition) > 0.1f)
                {
                    playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPosition, travelSpeed * Time.deltaTime);

                    // Make sure the rat always faces forward and remains upright
                    Quaternion targetRotation = Quaternion.LookRotation(targetPosition - playerTransform.position);
                    playerTransform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

                    yield return null;
                }
            }
        }

        // Move to final exit position
        playerTransform.position = exit.position;

        // Ensure the rat is looking at the exit before exiting
        playerTransform.rotation = Quaternion.LookRotation(exit.position - playerTransform.position);
        playerTransform.rotation = Quaternion.Euler(0f, playerTransform.eulerAngles.y, 0f); // Keep upright

        // Apply exit force
        rb.isKinematic = false;
        rb.freezeRotation = false;
        Vector3 exitDirection = (exit.position - (waypoints.Count > 0 ? waypoints[^1].position : entry.position)).normalized;
        rb.velocity = (exitDirection + Vector3.up * 0.5f) * Mathf.Clamp(exitForce, 1f, 10f);
        ratmovement.enabled = true;
        
        // Cooldown before re-enabling colliders
        yield return new WaitForSeconds(cooldownTime);
        exitTrigger.enabled = true;
    }
}

