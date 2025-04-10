using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class StageEnd : MonoBehaviour
{
    public bool exitTransition = false;
    public bool saveProgress = false;
    public int levelPosition = 0;
    public int labID = 1;

    private string overwriteFile;

    [SerializeField] private bool stageEnded;
    [Range(0f,10f)]
    [SerializeField] private float WaitTime = 5f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player" && !stageEnded)
        {
            Debug.Log("end stage");
            stageEnded = !stageEnded; //Why would we not just put true :(
            overwriteFile = Application.dataPath + "/levelsCleared.txt";
            if (File.Exists(overwriteFile))
            {


                if (int.Parse(GetLineAtIndex(0)) < levelPosition && saveProgress)
                {
                    OverWriteText(levelPosition.ToString());
                }
            }
            StartCoroutine(ReturnToMenu());
        }
    }
    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(WaitTime);
        SceneManager.LoadScene("Hub", LoadSceneMode.Single);
    }


    private string GetLineAtIndex(int index)
    {
        string[] lines = File.ReadAllLines(overwriteFile);

        if (index < lines.Length)
        {
            return lines[index];
        }

        return "0";
    }

    private void OverWriteText(string inputText)
    {
        if (!File.Exists(overwriteFile))
        {
            File.WriteAllText(path: overwriteFile, contents: inputText);
        }
        else
        {
            using (var writer = new StreamWriter(overwriteFile))
            {
                writer.WriteLine(inputText);
            }
        }
    }


}
