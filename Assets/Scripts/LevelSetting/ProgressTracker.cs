using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProgressTracker : MonoBehaviour
{
    public int levelsCompleted = 0;
    public int Lab1Completion = 0;
    public int Lab2Completion = 0;
    public int Lab3Completion = 0;


    public LevelSelectController FirstLabLvlSelect;
    public LevelSelectController SecondLabLvlSelect;
    public LevelSelectController ThirdLabLvlSelect;

    public List<string> levels;
    public StringLevelTeleport levelPipe;

    private string overwriteFile;


    // Start is called before the first frame update
    void Awake()
    {

        overwriteFile = Application.dataPath + "/levelsCleared.txt";
        if (File.Exists(overwriteFile))
            levelsCompleted = int.Parse(GetLineAtIndex(0));
        Lab1Completion = PlayerPrefs.GetInt("LabOneCompletion");
        Lab2Completion = PlayerPrefs.GetInt("LabTwoCompletion");
        Lab3Completion = PlayerPrefs.GetInt("LabThreeCompletion");

        Debug.Log("Levels completed: " + levelsCompleted);

        int lastLab = PlayerPrefs.GetInt("LastVisitedLab");

        //if (lastLab == 1)
        //{
        //    PlayerPrefs.SetInt("LabOneCompletion", Lab1Completion += 1);
        //    Lab1Completion = PlayerPrefs.GetInt("LabOneCompletion");
        //}
        //else if (lastLab == 2) {
        //    PlayerPrefs.SetInt("LabTwoCompletion", Lab2Completion += 1);
        //    Lab2Completion = PlayerPrefs.GetInt("LabTwoCompletion");

        //}
        //else if (lastLab == 3)
        //{
        //    PlayerPrefs.SetInt("LabThirdCompletion", Lab3Completion += 1);
        //    Lab3Completion = PlayerPrefs.GetInt("LabThreeCompletion");

        //}
        //PlayerPrefs.SetInt("LastVisitedLab", 0);
        //This code is horrible, last week stuff

        //FirstLabLvlSelect.levelsUnlocked = Lab1Completion;
        //SecondLabLvlSelect.levelsUnlocked = Lab2Completion;
        //ThirdLabLvlSelect.levelsUnlocked = Lab3Completion;


        OverWriteText(levelsCompleted.ToString());

        if (levelsCompleted < levels.Count)
        {
            levelPipe.level = levels[levelsCompleted];
        }
        
    }

    private void AppendText(string inputText)
    {
        if (!File.Exists(overwriteFile))
        {
            File.WriteAllText(path: overwriteFile, contents: inputText);
        }
        else
        {
            using (var writer = new StreamWriter(overwriteFile, append: true))
            {
                writer.WriteLine(inputText);

            }
        }
    }

    private void OverWriteText(string inputText)
    {
        if (!File.Exists(overwriteFile))
        {
            File.WriteAllText(path:overwriteFile, contents:inputText);
        }
        else
        {
            using (var writer = new StreamWriter(overwriteFile)) { 
            writer.WriteLine(inputText);
            }
        }
    }

    private string GetLineAtIndex(int index)
    {
        string[] lines = File.ReadAllLines(overwriteFile);

        if(index < lines.Length)
        {
            return lines[index];
        }
        
        return "0";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
