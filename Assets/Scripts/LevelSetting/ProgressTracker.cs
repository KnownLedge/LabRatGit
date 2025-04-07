using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Awake()
    {
        levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted");
        Lab1Completion = PlayerPrefs.GetInt("LabOneCompletion");
        Lab2Completion = PlayerPrefs.GetInt("LabTwoCompletion");
        Lab3Completion = PlayerPrefs.GetInt("LabThreeCompletion");

        Debug.Log("Levels completed: " + levelsCompleted);

        int lastLab = PlayerPrefs.GetInt("LastVisitedLab");

        if (lastLab == 1)
        {
            PlayerPrefs.SetInt("LabOneCompletion", Lab1Completion += 1);
            Lab1Completion = PlayerPrefs.GetInt("LabOneCompletion");
        }
        else if (lastLab == 2) {
            PlayerPrefs.SetInt("LabTwoCompletion", Lab2Completion += 1);
            Lab2Completion = PlayerPrefs.GetInt("LabTwoCompletion");

        }
        else if (lastLab == 3)
        {
            PlayerPrefs.SetInt("LabThirdCompletion", Lab3Completion += 1);
            Lab3Completion = PlayerPrefs.GetInt("LabThreeCompletion");

        }
        PlayerPrefs.SetInt("LastVisitedLab", 0);
        //This code is horrible, last week stuff

        FirstLabLvlSelect.levelsUnlocked = Lab1Completion;
        SecondLabLvlSelect.levelsUnlocked = Lab2Completion;
        ThirdLabLvlSelect.levelsUnlocked = Lab3Completion;

        if (levelsCompleted < levels.Count)
        {
            levelPipe.level = levels[levelsCompleted];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
