using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeToMenu : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene("LevelSelect");
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
