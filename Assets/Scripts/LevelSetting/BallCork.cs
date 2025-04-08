using System.Collections;
using UnityEngine;
using Cinemachine;

public class BallCork : MonoBehaviour
{
    [SerializeField] private AudioClip openSound;
    private AudioSource audioSource;
    [SerializeField] private Animator corkAnimator;
    [SerializeField] private PipeTransport pipeTransport;
    [SerializeField] private GameObject cork; 
    [SerializeField] private CinemachineBrain cinemachineBrain; // Reference to your main camera's Cinemachine Brain
    [SerializeField] private float focusDuration = 3f; // Time to focus on the cork
    [SerializeField] private float fadeDuration = 3f; // Time for cork to dissapear

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
            if (openSound != null)
            {
                audioSource.clip = openSound;
                audioSource.Play();
            }

            corkAnimator.SetTrigger("PlayAnimation");
            StartCoroutine(FadeOutCork());

            pipeTransport.triggerA.enabled = true;
            pipeTransport.triggerB.enabled = true;

            StartCoroutine(FocusOnCork());
        }
    }

    private IEnumerator FadeOutCork()
    {
        yield return new WaitForSeconds(2f); // Wait for the animation to finish
        Renderer renderer = cork.GetComponent<Renderer>();
        Material material = renderer.material;
        Color color = material.color;
        float startAlpha = color.a;

        // Ensure the material supports transparency
        material.SetFloat("_Mode", 2);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, 0, normalizedTime);
            material.color = color;
            yield return null;
        }

        color.a = 0;
        material.color = color;
        cork.SetActive(false); // Optionally disable the cork GameObject after fading out
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
        tempCamera.LookAt = cork.transform;
        tempCamera.Follow = cork.transform;

        tempCamera.transform.position = Camera.main.transform.position;

        Debug.Log("Cork Position: " + cork.transform.position);
        Debug.Log("Camera Position: " + tempCamera.transform.position);

        // Manually adjust the camera's rotation so it faces the cork
        Vector3 directionToCork = cork.transform.position - tempCamera.transform.position;
        tempCamera.transform.rotation = Quaternion.LookRotation(directionToCork); 

        Debug.Log("Camera's Look Direction: " + directionToCork.normalized);

        yield return new WaitForSeconds(focusDuration);

        Destroy(tempCamObject);
    }
}
