using UnityEngine;

public class HatLoader : MonoBehaviour
{
    private const string PLAYER_PREFS_HAT = "SelectedHat";

    [SerializeField] private CustomizationManager customizationManager; // Reference to the CustomizationManager

    void Start()
    {
        // Load the saved hat and apply it
        LoadSavedHat();
    }

    void LoadSavedHat()
    {
        // Check if there is a saved hat in PlayerPrefs
        if (PlayerPrefs.HasKey(PLAYER_PREFS_HAT))
        {
            int savedHatIndex = PlayerPrefs.GetInt(PLAYER_PREFS_HAT); // Get saved hat index from PlayerPrefs

            // Apply the saved hat index using the CustomizationManager
            customizationManager.SetSelectedHat(savedHatIndex);
        }
    }
}
