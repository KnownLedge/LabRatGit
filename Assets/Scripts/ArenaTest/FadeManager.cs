using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        fadeImage.enabled = false;
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        if(fadeImage.enabled == false)
            fadeImage.enabled = true;
        StartCoroutine(FadeOutCoroutine(sceneName));
    }

    private IEnumerator FadeOutCoroutine(string sceneName)
    {
        yield return StartCoroutine(Fade(1)); //Fade to black
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator Fade(float targetAlpha)
    {
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
    }
}
