using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateDoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnim = null;      // Animator for the door
    [SerializeField] private Animator plateAnim = null;     // Animator for the plate

    [SerializeField] private bool isPlatePressed = false;                    // Check if plate is currently pressed
    private Coroutine doorCloseCoroutine = null; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPlatePressed)
            {
                plateAnim.Play("PlatePressed", 0, 0.0f);    // Play plate pressed animation
                doorAnim.Play("DoorOpen", 0, 0.0f);         // Open door
                isPlatePressed = true;                      // Mark plate as pressed
            
                // If a coroutine to close the door is running, stop it
                if (doorCloseCoroutine != null)
                {
                    StopCoroutine(doorCloseCoroutine);
                    doorCloseCoroutine = null;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isPlatePressed)
            {
                plateAnim.Play("PlateNotPressed", 0, 0.0f); // Play plate released animation
                isPlatePressed = false;                     // Reset plate state
            
                // Start a coroutine to close the door after 3 seconds
                doorCloseCoroutine = StartCoroutine(CloseDoorAfterDelay(2f));
            }
        }
    }

    private IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);             // Wait for 3 seconds
        doorAnim.Play("DoorClose", 0, 0.0f);                // Close the door
    }
}
