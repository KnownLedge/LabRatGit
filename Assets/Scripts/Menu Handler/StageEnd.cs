using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageEnd : MonoBehaviour
{
    [SerializeField] private bool stageEnded;
    [Range(0f,10f)]
    [SerializeField] private float WaitTime = 5f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player" && !stageEnded)
        {
            Debug.Log("end stage");
            stageEnded = !stageEnded;
            StartCoroutine(ReturnToMenu());
        }
    }
    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(WaitTime);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }


}
