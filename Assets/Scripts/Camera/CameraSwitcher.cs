using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] public Camera[] cameras;
    [SerializeField] public int currentCameraIndex = 0;
    [SerializeField] private Collider triggerNext;
    [SerializeField] private Collider triggerPrev;
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
        if (other == triggerNext || other == triggerPrev)
        {
            StartCoroutine(DisableTriggersForSeconds(3f));
            StartCoroutine(SwitchCameraWithDelay(other == triggerNext));
        }
    }

    public IEnumerator SwitchCameraWithDelay(bool isNext)
    {
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

    IEnumerator DisableTriggersForSeconds(float seconds)
    {
        triggerNext.enabled = false;
        triggerPrev.enabled = false;
        yield return new WaitForSeconds(seconds);
        triggerNext.enabled = true;
        triggerPrev.enabled = true;
    }
}
