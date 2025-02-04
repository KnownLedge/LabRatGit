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

        entryTrigger.enabled = false;
        exitTrigger.enabled = false;

        List<Transform> waypoints = (entry == entryA) ? waypointsA : waypointsB;

        //rb.isKinematic = true;
        //rb.freezeRotation = true;
        player.transform.position = entry.position;

        Vector3 lookTarget = (waypoints != null && waypoints.Count > 0) ? waypoints[0].position : exit.position;
        player.transform.rotation = Quaternion.LookRotation(lookTarget - player.transform.position);
        player.transform.rotation = Quaternion.Euler(0f, player.transform.eulerAngles.y, 0f);

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogWarning("No waypoints assigned! Moving directly to the exit.");
        }
        else
        {
            foreach (Transform target in waypoints)
            {
                Vector3 targetPosition = target.position;
                while (Vector3.Distance( player.transform.position, targetPosition) > 0.1f)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, travelSpeed * Time.deltaTime);

                    Quaternion targetRotation = Quaternion.LookRotation(targetPosition - player.transform.position);
                    player.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

                    yield return null;
                }
            }
        }

        player.transform.position = exit.position;

        player.transform.rotation = Quaternion.LookRotation(exit.position - player.transform.position);
        player.transform.rotation = Quaternion.Euler(0f, player.transform.eulerAngles.y, 0f); // Keep upright

        //rb.isKinematic = false;
        //rb.freezeRotation = false;
        Vector3 exitDirection = (exit.position - (waypoints.Count > 0 ? waypoints[^1].position : entry.position)).normalized;
        rb.velocity = (exitDirection + Vector3.back) * Mathf.Clamp(exitForce, 1f, 100f);
        ratmovement.enabled = true;
        yield return new WaitForSeconds(cooldownTime);
        //ratmovement.enabled = true;
        entryTrigger.enabled = true;
        exitTrigger.enabled = true;
    }
}

