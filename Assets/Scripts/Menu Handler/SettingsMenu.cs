using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenuUI;

    public Slider musicSlider;
    public Slider sfxSlider;


    void Start()
    {
        musicSlider.value = AudioManager.Instance.musicVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;

        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);

        settingsMenuUI.SetActive(false);
    }

    public void Open()
    {
        settingsMenuUI.SetActive(true);
    }

    public void Close()
    {
        settingsMenuUI.SetActive(false);
    }
}
