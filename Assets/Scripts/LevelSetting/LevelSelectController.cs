using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    public GameObject[] levelSets;
    public string[] levelSceneNames;
    public GameObject laddersRef;
    public GameObject pipePrefab;
    public int totalLevels = 1;
    public int levelsUnlocked = 1;
    public int nextLevel = 1;

        public float fadeTime = 3f;
    private float fadeTimer = 0f;

        public Image fadeImage;



    void Start()
    {
        levelSets[levelsUnlocked - 1].SetActive(true);

        Transform[] pipeEntrances = levelSets[levelsUnlocked - 1].transform.GetComponentsInChildren<Transform>();

        for(int i = 1; i < pipeEntrances.Length; i++){
           GameObject newPipe;
           newPipe =  Instantiate(pipePrefab, pipeEntrances[i]);
          Transform newPipeEntrance = newPipe.transform.Find("EntryA"); //bad code

           LevelTransition pipeLevel = newPipeEntrance.GetComponent<LevelTransition>();

           pipeLevel.level = i;
           pipeLevel.levelControl = transform.GetComponent<LevelSelectController>();
            // This is a really stupid way to reference this very script, but im not gonna spend long researching this 
        }
        if(levelsUnlocked < 5){
            laddersRef.SetActive(false);
        }
    }

    public void LevelTransition(int level){
        nextLevel = level;
        Debug.Log("Still goin!");
            StartCoroutine(ScreenFade());
    }

    IEnumerator ScreenFade(){
        while(fadeTimer < fadeTime){
            Debug.Log("This is happening too!" + fadeTimer);
            fadeTimer += 0.001f;

            yield return new WaitForSeconds(0.001f);
            float colAlpha = 1 - ((fadeTime - fadeTimer) / fadeTime);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, colAlpha);
        }
     //   sceneCheckPoint = 0;
     Debug.Log("and so is this!");
            SceneManager.LoadScene(levelSceneNames[nextLevel - 1]);
    }
}
