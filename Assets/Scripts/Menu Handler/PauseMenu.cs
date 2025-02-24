using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public bool isPaused = false;

    void Start()
    {
        // Ensure the game starts unpaused
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        // If the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If the game is paused
            if (isPaused)
            {
                // Resume the game
                Resume();
            }
            else // If the game is not paused
            {
                // Pause the game
                Pause();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneManager.LoadScene("Title");
        }

    }

    // Function to resume the game
    public void Resume()
    {
        // Hide the pause menu UI
        pauseMenuUI.SetActive(false);
        // Resume game time
        Time.timeScale = 1f;
        // Update the paused state to false
        isPaused = false;
    }

    // Function to pause the game
    void Pause()
    {
        // Show the pause menu UI
        pauseMenuUI.SetActive(true);
        // Freeze game time
        Time.timeScale = 0f;
        // Update the paused state to true
        isPaused = true;
    }
}