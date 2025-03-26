using System.Collections;
using UnityEngine;
using Cinemachine;

public class BallCork : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    private AudioSource audioSource;
    [SerializeField] private Animator corkAnimator;
    [SerializeField] private PipeTransport pipeTransport;
    [SerializeField] private Transform Cork; 
    [SerializeField] private CinemachineBrain cinemachineBrain; // Reference to your main camera's Cinemachine Brain
    [SerializeField] private float focusDuration = 3f; // Time to focus on the cork

    private CinemachineVirtualCamera tempCamera;

    private void Start()
    {
        pipeTransport.triggerA.enabled = false;
        pipeTransport.triggerB.enabled = false;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    // When the ball enters the trigger, play the cork animation and enable pipe triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (collectSound != null)
            {
                audioSource.clip = collectSound;
                audioSource.Play();
            }

            corkAnimator.SetTrigger("PlayAnimation");

            pipeTransport.triggerA.enabled = true;
            pipeTransport.triggerB.enabled = true;

            StartCoroutine(FocusOnCork());
        }
    }

    private IEnumerator FocusOnCork()
    {
        if (tempCamera != null)
        {
            Destroy(tempCamera.gameObject);
        }

        // Create a temporary Cinemachine Virtual Camera
        GameObject tempCamObject = new GameObject("TempVirtualCamera");
        tempCamera = tempCamObject.AddComponent<CinemachineVirtualCamera>();

        tempCamera.Priority = 20;
        tempCamera.LookAt = Cork;
        tempCamera.Follow = Cork;

        tempCamera.transform.position = Camera.main.transform.position;

        Debug.Log("Cork Position: " + Cork.position);
        Debug.Log("Camera Position: " + tempCamera.transform.position);

        // Manually adjust the camera's rotation so it faces the cork
        Vector3 directionToCork = Cork.position - tempCamera.transform.position;
        tempCamera.transform.rotation = Quaternion.LookRotation(directionToCork); 

        Debug.Log("Camera's Look Direction: " + directionToCork.normalized);

        yield return new WaitForSeconds(focusDuration);

        Destroy(tempCamObject);
    }
}
