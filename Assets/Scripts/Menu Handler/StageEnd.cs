using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageEnd : MonoBehaviour
{
    public bool exitTransition = false;
    public bool saveProgress = false;
    public int levelPosition = 0;
    public int labID = 1;


    [SerializeField] private bool stageEnded;
    [Range(0f,10f)]
    [SerializeField] private float WaitTime = 5f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player" && !stageEnded)
        {
            Debug.Log("end stage");
            stageEnded = !stageEnded; //Why would we not just put true :(

            if (PlayerPrefs.GetInt("LevelsCompleted") < levelPosition && saveProgress)
            {
                PlayerPrefs.SetInt("LevelsCompleted", levelPosition);
                PlayerPrefs.SetInt("LastVisitedLab", labID);
            }

            StartCoroutine(ReturnToMenu());
        }
    }
    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(WaitTime);
        SceneManager.LoadScene("Hub", LoadSceneMode.Single);
    }


}
