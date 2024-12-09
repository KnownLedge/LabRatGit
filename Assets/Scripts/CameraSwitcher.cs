using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] cameras; // Array of cameras
    private int currentCameraIndex = 0;  // Tracks the current camera index
    public Ratmovement ratmovement; // Reference to the rat movement script

    // Reference to the Animator controlling camera transitions
    [SerializeField] private Animator cameraAnimator;

    private void Start()
    {
        ratmovement = GetComponent<Ratmovement>();
        // Initially, enable the first camera in the array
        Debug.Log("Starting CameraSwitcher. Enabling camera at index: " + currentCameraIndex);
        SwitchCamera(currentCameraIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CameraTriggerForward"))
        {
            // Switch to next camera on the list (forward)
            int nextCameraIndex = (currentCameraIndex + 1) % cameras.Length;
            SwitchCamera(nextCameraIndex);
        }
        else if (other.CompareTag("CameraTriggerBackward"))
        {
            // Switch to previous camera on the list (backward)
            int prevCameraIndex = Mathf.Max(currentCameraIndex - 1, 0);
            SwitchCamera(prevCameraIndex);
        }
    }

    private void SwitchCamera(int newCameraIndex)
    {
        // Disable movement while the camera is switching
        ratmovement.moveState = false; 

        // Disable current camera
        cameras[currentCameraIndex].SetActive(false);

        // Enable new camera
        cameras[newCameraIndex].SetActive(true);

        // Trigger the existing "change" animation
        cameraAnimator.SetTrigger("Change");

        // Update the current camera index
        currentCameraIndex = newCameraIndex;

        // Log the camera switch for debugging
        Debug.Log("Switched to camera: " + (newCameraIndex + 1));

        // Start a coroutine to wait for the animation to finish before re-enabling movement
        StartCoroutine(WaitForAnimation());
    }

    // Coroutine to wait for the animation to finish and then re-enable movement
    private IEnumerator WaitForAnimation()
    {
        // Wait for the duration of the "Change" animation to finish
        float animationDuration = cameraAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationDuration);

        // Re-enable movement after the animation has finished
        ratmovement.moveState = true;
        Debug.Log("Movement enabled again.");
    }
}
