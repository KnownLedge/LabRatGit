using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    [SerializeField] public Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (fadeImage != null)
        {
            // Fully black at start
            fadeImage.color = new Color(0, 0, 0, 1f);
            fadeImage.enabled = true;
        }
        else
        {
            Debug.LogError("[FadeManager] fadeImage is not assigned.");
        }
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        if (fadeImage != null)
        {
            fadeImage.enabled = true;
            StartCoroutine(FadeOutCoroutine(sceneName));
        }
    }

    private IEnumerator FadeOutCoroutine(string sceneName)
    {
        yield return StartCoroutine(Fade(1)); // Fade to black
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator RespawnFade()
    {
        if (fadeImage != null)
        {
            fadeImage.enabled = true;
            yield return StartCoroutine(Fade(0)); // Fade to transparent
        }
    }

    public IEnumerator Fade(float targetAlpha)
    {
        Debug.Log("[FadeManager] Fading to alpha " + targetAlpha);
        fadeImage.enabled = true;

        float timeElapsed = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (timeElapsed < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, targetColor, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = targetColor;

        if (targetAlpha == 0f)
        {
            fadeImage.enabled = false;
        }
    }
}
