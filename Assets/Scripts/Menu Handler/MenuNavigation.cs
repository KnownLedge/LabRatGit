using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public PauseMenu pauseMenuScript;

  
    public void ResumeGame()
    {
        if (pauseMenuScript != null)
        {
            pauseMenuScript.Resume(); 
        }
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }


    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
