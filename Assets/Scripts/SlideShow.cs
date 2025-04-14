using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlideShow : MonoBehaviour
{
    public GameObject[] images;
    private int i = 0;
    public string exitScene = "Hub";

    private void Start()
    {
        images[i].SetActive(true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (i == images.Length - 1)
            {
                SceneManager.LoadScene(exitScene, LoadSceneMode.Single);
            }
            else
            {
              //  images[i].gameObject.SetActive(false);
                i++;
                images[i].gameObject.SetActive(true);
            }
        }
    }
}
