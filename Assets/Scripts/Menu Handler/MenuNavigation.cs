using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public PauseMenu pauseMenuScript;
    public SettingsMenu settingsMenuScript;

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
        if (pauseMenuScript != null)
        {
            pauseMenuScript.Hide();
        }


        if (settingsMenuScript != null)
        {
            settingsMenuScript.Open();
        }
    }

    public void ReturnToPauseMenu()
    {
        if (settingsMenuScript != null)
        {
            settingsMenuScript.Close();
        }

        if (pauseMenuScript != null)
        {
            pauseMenuScript.Pause();
        }
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
