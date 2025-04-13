using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BallCollectable : MonoBehaviour
{
    [SerializeField] private GameObject collectable;
    [SerializeField] private CinemachineBrain cinemachineBrain; // Reference to the main camera's Cinemachine Brain
    [SerializeField] private float focusDuration = 3f; // Time to focus on the collectable

    private CinemachineVirtualCamera tempCamera;

    private void Start()
    {
        collectable.SetActive(false);
    }

    // When ball enters the trigger, the collectable will be set to active and camera will focus on it
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            collectable.SetActive(true);
            StartCoroutine(FocusOnCollectable());
        }
    }

    private IEnumerator FocusOnCollectable()
    {
        if (tempCamera != null)
        {
            Destroy(tempCamera.gameObject);
        }

        // Create a temporary Cinemachine Virtual Camera
        GameObject tempCamObject = new GameObject("TempVirtualCamera");
        tempCamera = tempCamObject.AddComponent<CinemachineVirtualCamera>();

        tempCamera.Priority = 20; 
        tempCamera.LookAt = collectable.transform;
        tempCamera.Follow = collectable.transform; 

        tempCamera.transform.position = Camera.main.transform.position;

        Debug.Log("Collectable Position: " + collectable.transform.position);
        Debug.Log("Camera Position: " + tempCamera.transform.position); 

        // Manually adjust the camera's rotation so it faces the collectable
        Vector3 directionToCollectable = collectable.transform.position - tempCamera.transform.position;
        tempCamera.transform.rotation = Quaternion.LookRotation(directionToCollectable);

        Debug.Log("Camera's Look Direction: " + directionToCollectable.normalized); 

        yield return new WaitForSeconds(focusDuration);

        Destroy(tempCamObject);
    }
}
