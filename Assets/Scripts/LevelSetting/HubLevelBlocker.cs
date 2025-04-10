using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HubLevelBlocker : MonoBehaviour
{
    private string overwriteFile = Application.dataPath + "/levelsCleared.txt";
    void Start()
    {
        string[] lines = File.ReadAllLines(overwriteFile);
        if (int.Parse(lines[0]) > 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject.GetComponentInParent<PipeTransport>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
