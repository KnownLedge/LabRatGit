using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras;           // Array of cameras to switch between
    private int currentCameraIndex = 0; // Tracks the current active camera
    public Animator animator;          // Animator for triggering the "Change" animation
    public string animationTrigger = "Change"; // Name of the animation trigger
    public Ratmovement ratMovement;    // Reference to the Ratmovement script

    void Start()
    {
        // Disable all cameras except the first one
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == currentCameraIndex);
        }

        if (ratMovement == null)
        {
            Debug.LogWarning("Ratmovement script not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collides with a trigger tagged "CameraTrigger"
        if (other.CompareTag("CameraTrigger"))
        {
            PlayChangeAnimation();
        }
    }

    void PlayChangeAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(animationTrigger);
            Debug.Log("Animation trigger sent: " + animationTrigger);

            // Disable rat movement at the start of the animation
            if (ratMovement != null)
            {
                ratMovement.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning("Animator is not assigned. Switching camera without animation.");
        }

        // Switch camera after animation (adjust delay based on animation duration)
        Invoke(nameof(SwitchToNextCamera), 1f); // Adjust "1f" to match your animation duration
    }

    void SwitchToNextCamera()
    {
        // Disable the current camera
        cameras[currentCameraIndex].gameObject.SetActive(false);

        // Move to the next camera (loop back to the first camera if at the end)
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;

        // Enable the next camera
        cameras[currentCameraIndex].gameObject.SetActive(true);

        // Re-enable rat movement after the camera switch
        if (ratMovement != null)
        {
            ratMovement.enabled = true;
        }

        Debug.Log("Switched to camera: " + cameras[currentCameraIndex].name);
    }
}
