using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    public int levelsCompleted = 0;
    public int Lab1Completion = 0;
    public int Lab2Completion = 0;
    public int Lab3Completion = 0;
    // Start is called before the first frame update
    void Start()
    {
        levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted");
        Lab1Completion = PlayerPrefs.GetInt("LabOneCompletion");
        Lab2Completion = PlayerPrefs.GetInt("LabTwoCompletion");
        Lab3Completion = PlayerPrefs.GetInt("LabThreeCompletion");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
