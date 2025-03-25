using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    [SerializeField] public Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Arena_Test")
        {
            fadeImage.enabled = true;
            StartCoroutine(Fade(0));
        }
    }

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

    public IEnumerator RespawnFade()
    {
        if (fadeImage.enabled == false)
            fadeImage.enabled = true;
        yield return StartCoroutine(Fade(0));
    }

    public IEnumerator Fade(float targetAlpha)
    {
        Debug.Log("Fading");
        float timeElapsed = 0f;
        if (targetAlpha == 0)
        {
            Color startColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);
            Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

            while (timeElapsed < fadeDuration)
            {
                fadeImage.color = Color.Lerp(startColor, targetColor, timeElapsed / fadeDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            fadeImage.color = targetColor;
        }
        else
        {
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
}
