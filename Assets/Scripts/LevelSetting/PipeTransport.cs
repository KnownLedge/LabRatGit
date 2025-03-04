using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PipeTransport : MonoBehaviour
{
    [SerializeField] private Ratmovement ratmovement;
    [SerializeField] private CameraSwitcher cameraSwitcher;
    
    [Header("Settings")]
    [SerializeField] private Transform entryA;
    [SerializeField] private Transform entryB;
    [SerializeField] private Collider triggerA;
    [SerializeField] private Collider triggerB;
    [SerializeField] private float travelSpeed = 10f;
    [SerializeField] private float accelerationFactor = 5f;
    [SerializeField] private float exitForce = 5f;
    [SerializeField] private float cooldownTime = 1.5f;

    [Header("Waypoints")]
    [SerializeField] private List<Transform> waypointsA; //A->B
    [SerializeField] private List<Transform> waypointsB; //B->A

    [Header("Effects and Sounds")]
    [SerializeField] private ParticleSystem entryEffect;
    [SerializeField] private ParticleSystem exitEffect;
    [SerializeField] private AudioClip enterSound;
    [SerializeField] private AudioClip exitSound; 
    [SerializeField] private AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform entryPoint = GetClosestEntry(other.transform.position);
            Transform exitPoint = (entryPoint == entryA) ? entryB : entryA;
            Collider entryTrigger = (entryPoint == entryA) ? triggerA : triggerB;
            Collider exitTrigger = (entryPoint == entryA) ? triggerB : triggerA;

            ratmovement.enabled = false;

            if (entryPoint == entryA && entryEffect != null)
            {
                entryEffect.transform.position = entryA.position; 
                entryEffect.Play();
            }
            else if (entryPoint == entryB && entryEffect != null)
            {
                entryEffect.transform.position = entryB.position;
                entryEffect.Play();
            }

            if (enterSound != null && audioSource != null) 
                audioSource.PlayOneShot(enterSound);

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

        rb.isKinematic = true;
        rb.freezeRotation = true;
        player.transform.position = entry.position;

        List<Transform> waypoints = (entry == entryA) ? waypointsA : waypointsB;

        float currentSpeed = 0f;

        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogWarning("No waypoints assigned! Moving directly to the exit.");
        }
        else
        {
            foreach (Transform target in waypoints)
            {
                Vector3 targetPosition = target.position;
                while (Vector3.Distance(player.transform.position, targetPosition) > 0.1f)
                {
                    currentSpeed = Mathf.MoveTowards(currentSpeed, travelSpeed, accelerationFactor * Time.deltaTime);
                    player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, currentSpeed * Time.deltaTime);

                    Quaternion targetRotation = Quaternion.LookRotation(targetPosition - player.transform.position);
                    player.transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

                    yield return null;
                }
            }
        }

        player.transform.position = exit.position;

        if (exitEffect != null) exitEffect.Play();
        if (exitSound != null && audioSource != null) audioSource.PlayOneShot(exitSound);

        rb.isKinematic = false;

        Vector3 exitDirection = player.transform.forward;
        rb.AddForce(exitDirection * exitForce, ForceMode.Impulse);

        rb.freezeRotation = false;
        ratmovement.enabled = true;

        if(cameraSwitcher != null)
        {
           StartCoroutine(SwitchCameraAndEnableAxisCamera());
           Debug.Log("SwitchCameraAndEnableAxisCamera");
        }

        yield return new WaitForSeconds(cooldownTime);
        entryTrigger.enabled = true;
        exitTrigger.enabled = true;
    }

    private IEnumerator SwitchCameraAndEnableAxisCamera()
    {
        yield return new WaitForSeconds(0.5f);
        Camera activeCamera = cameraSwitcher.cameras[cameraSwitcher.currentCameraIndex];
        AxisCamera axisCamera = activeCamera.GetComponent<AxisCamera>();
        if (axisCamera != null)
        {
            axisCamera.enabled = true;
        }
        Debug.Log("AxisCamera enabled");
    }
}
