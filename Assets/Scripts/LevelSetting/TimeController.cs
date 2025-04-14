using UnityEngine;

public class TimeController : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsOfType<TimeController>().Length > 1)
        {
            Debug.LogWarning("Multiple Time Controller instances detected!");
            Destroy(gameObject);  // Destroy the duplicate manager
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetTimeScale(1f, "Normal Speed");
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetTimeScale(0.75f, "75% Speed");
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SetTimeScale(0.5f, "Half Speed");
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SetTimeScale(0.25f, "Quarter Speed");
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            SetTimeScale(0.1f, "10% Speed");
        else if (Input.GetKeyDown(KeyCode.Alpha0))
            SetTimeScale(0f, "Paused");
    }

    void SetTimeScale(float newScale, string label)
    {
        Time.timeScale = newScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        Debug.Log("Time Scale set to " + newScale + " (" + label + ")");
    }
}
