using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scenemanager : MonoBehaviour
{
    public Scene nextScene;
    public string nextSceneName;
    public float fadeTime = 3f;
    private float fadeTimer = 0f;
    public static int scenePhase = 1;
    public static int sceneCheckPoint = 1;

    public Image fadeImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SceneTransition(){
            StartCoroutine(ScreenFade());
    }

    IEnumerator ScreenFade(){
        while(fadeTimer < fadeTime){
            fadeTimer += 0.001f;

            yield return new WaitForSeconds(0.001f);
            float colAlpha = 1 - ((fadeTime - fadeTimer) / fadeTime);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, colAlpha);
        }
        sceneCheckPoint = 0;
            SceneManager.LoadScene(nextSceneName);
    }

}
