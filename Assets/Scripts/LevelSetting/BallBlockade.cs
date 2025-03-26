using System.Collections;
using UnityEngine;
using Cinemachine;

public class BallBlockade : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private Animator blockadeAnimator;
    [SerializeField] private Transform blockade; 
    [SerializeField] private Transform player; 
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera; 
    [SerializeField] private float focusDuration = 3f; // How long to focus on blockade

    private AudioSource audioSource;
    private int ballCount = 0;
    private Transform originalLookAt;
    private Transform originalFollow;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;

        // Save the original LookAt and Follow targets of the Cinemachine camera
        if (cinemachineCamera != null)
        {
            originalLookAt = cinemachineCamera.LookAt;
            originalFollow = cinemachineCamera.Follow;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (collectSound != null)
            {
                audioSource.clip = collectSound;
                audioSource.Play();
            }

            ballCount++;
            Debug.Log("Ball Count: " + ballCount);

            if (ballCount == 3)
            {
                StartCoroutine(FocusOnBlockade());
                blockadeAnimator.SetTrigger("PlayAnimation");
            }
        }
    }

    private IEnumerator FocusOnBlockade()
    {
        if (cinemachineCamera == null) yield break;

        // Change the camera target to the blockade
        cinemachineCamera.LookAt = blockade;
        cinemachineCamera.Follow = blockade;

        yield return new WaitForSeconds(focusDuration); 

        // Restore the original camera focus back to the player
        cinemachineCamera.LookAt = originalLookAt;
        cinemachineCamera.Follow = originalFollow;
    }
}
