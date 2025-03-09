using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] public Camera[] cameras;
    [SerializeField] public int currentCameraIndex = 0;
    [SerializeField] private Collider[] nextTriggers;  // Array for next triggers
    [SerializeField] private Collider[] prevTriggers;  // Array for previous triggers
    [SerializeField] private float cameraSwitchDelay = 0.5f;

    void Start()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == currentCameraIndex);

            AxisCamera axisCamera = cameras[i].GetComponent<AxisCamera>();
            if (axisCamera != null)
            {
                axisCamera.enabled = i == currentCameraIndex;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the triggered collider is in the nextTriggers array
        if (System.Array.Exists(nextTriggers, trigger => trigger == other))
        {
            //StartCoroutine(DisableTriggersForSeconds(3.5f));
            StartCoroutine(SwitchCameraWithDelay(true)); // Go next
        }
        // Check if the triggered collider is in the prevTriggers array
        else if (System.Array.Exists(prevTriggers, trigger => trigger == other))
        {
           //StartCoroutine(DisableTriggersForSeconds(3.5f));
            StartCoroutine(SwitchCameraWithDelay(false)); // Go previous
        }
    }

    public IEnumerator SwitchCameraWithDelay(bool isNext)
    {
        DisableTriggers();
        yield return new WaitForSeconds(cameraSwitchDelay);

        Camera activeCamera = cameras[currentCameraIndex];
        AxisCamera axisCamera = activeCamera.GetComponent<AxisCamera>();
        if (axisCamera != null)
        {
            axisCamera.enabled = false;
        }

        cameras[currentCameraIndex].gameObject.SetActive(false);

        if (isNext)
        {
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
        }
        else
        {
            currentCameraIndex = (currentCameraIndex - 1 + cameras.Length) % cameras.Length;
        }

        cameras[currentCameraIndex].gameObject.SetActive(true);

        Debug.Log("Switched to camera: " + cameras[currentCameraIndex].name);
    }

    public void DisableTriggers()
    {
        foreach (Collider trigger in nextTriggers)
        {
            trigger.enabled = false;
        }
        foreach (Collider trigger in prevTriggers)
        {
            trigger.enabled = false;
        }
    }

    public void EnableTriggers()
    {
        foreach (Collider trigger in nextTriggers)
        {
            trigger.enabled = true;
        }
        foreach (Collider trigger in prevTriggers)
        {
            trigger.enabled = true;
        }
    }
}
