using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
//add this script to a button and choose a scene to load thats been added to the build menu
//click the " + " and drag the script onto the section marked "None (Object)"
//then click "No Function" and select "MenuButtonManager" then select "LoadScene ()"
public class MenuButtonManager : MonoBehaviour
{
    [SerializeField] private SceneAsset sceneToLoad;//dropdown menu to choose a scene you want it to load (needs to be added to the build menu to work)
    
    public void LoadScene()
    {
        if (sceneToLoad != null)//checks for scene selected
        {
            string sceneName = sceneToLoad.name;
            if (sceneName == "Quit")
            {
                Application.Quit();//game will close if selected
            }
            else
            {
                SceneManager.LoadScene(sceneName);//loads the scene chosen in inspector menu
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
