using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;
//add this script to a button and choose a scene to load thats been added to the build menu
//click the " + " and drag the script onto the section marked "None (Object)"
//then click "No Function" and select "MenuButtonManager" then select "LoadScene ()"
public class MenuButtonManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;//Change to string to be more compatible with the build
    
    public void LoadScene()
    {
        if (sceneToLoad != null)//checks for scene selected
        {
            if (sceneToLoad == "Quit")
            {
                Application.Quit();//game will close if selected
            }else if(sceneToLoad == "NewGame")
            {
                string overwriteFile = Application.dataPath + "/levelsCleared.txt";
                if (!File.Exists(overwriteFile))
                {
                    File.WriteAllText(path: overwriteFile, contents: "0");
                }
                SceneManager.LoadScene("Hub");
                Time.timeScale = 1f;
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);//loads the scene chosen in inspector menu
            }
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogError("SCENE NOT LOADED! Have you selected a scene and has the scene been added to the build settings?");//unity shows its own error anyways
            //shows error if no scene chosen or not added to build menu
        }
    }
}
