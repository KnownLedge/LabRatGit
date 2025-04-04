using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomizationManager : MonoBehaviour
{
    private const string PLAYER_PREFS_HAT = "SelectedHat";

    [Header("Hat Customization")]
    [SerializeField] private GameObject[] hats;
    [SerializeField] private Sprite[] hatSprites;

    [Header("UI Elements: Buttons")]
    [SerializeField] private Button leftButton; 
    [SerializeField] private Button rightButton; 
    [SerializeField] private Button applyButton;

    [Header("UI Elements: Images")]
    [SerializeField] private Image centerImage; 
    [SerializeField] private Image leftImage; 
    [SerializeField] private Image rightImage; 

    [Header("UI Elements: Objects")]
    [SerializeField] private GameObject leftObject;
    [SerializeField] private GameObject centerObject; 
    [SerializeField] private GameObject rightObject;

    [Header("UI Elements: Transitions")]
    [SerializeField] private float transitionDuration = 1.0f;

    [Header("References")]
    [SerializeField] private GameObject customizationMenu;
    [SerializeField] private Ratmovement ratMovement;
    
    // Camera Animator
    [SerializeField] private Animator cameraAnimator;
    
    private int selectedHatIndex = 0;
    private bool playerInZone = false;
    private bool isMenuActive = false;

    void Awake()
    {
        leftButton.onClick.AddListener(() => ChangeSelection(-1));
        rightButton.onClick.AddListener(() => ChangeSelection(1));
        applyButton.onClick.AddListener(ApplyHat);

        LoadCustomization();
        customizationMenu.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered customization zone.");
            playerInZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited customization zone.");
            playerInZone = false;
        }
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            ToggleMenu(!isMenuActive);
        }
    }

    void ToggleMenu(bool state)
    {
        isMenuActive = state;
        customizationMenu.SetActive(state);

        if (ratMovement != null)
        {
            ratMovement.enabled = !state;
        }

        if (state)
        {
            cameraAnimator.enabled = true;
            cameraAnimator.SetTrigger("GoToCustomization");
        }
        else
        {
            cameraAnimator.SetTrigger("GoBackToOriginal");
            StartCoroutine(DisableAnimatorAfterAnimation());
        }
    }

    private IEnumerator DisableAnimatorAfterAnimation()
    {   
        yield return new WaitForSeconds(2);
        cameraAnimator.enabled = false;
    }

    void ChangeSelection(int direction)
    {
        if (hats == null || hats.Length == 0)
        {
            Debug.LogError("Hats array is empty or not assigned!");
            return;
        }

        selectedHatIndex = (selectedHatIndex + direction + hats.Length) % hats.Length;
        UpdateUI();
    }

    void ApplyHat()
    {
        ActivateSelectedHat();
        PlayerPrefs.SetInt(PLAYER_PREFS_HAT, selectedHatIndex);
        PlayerPrefs.Save();
    }

    void LoadCustomization()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_HAT))
        {
            selectedHatIndex = PlayerPrefs.GetInt(PLAYER_PREFS_HAT);
        }
        ActivateSelectedHat();
        UpdateUI();
    }

    void ActivateSelectedHat()
    {
        foreach (var hat in hats)
        {
            hat.SetActive(false);
        }

        if (hats.Length > selectedHatIndex && selectedHatIndex >= 0)
        {
            hats[selectedHatIndex].SetActive(true);
        }
    }

    void UpdateUI()
    {
        if (hatSprites == null || hatSprites.Length == 0)
        {
            Debug.LogError("Hat sprites array is empty or not assigned!");
            return;
        }

        centerImage.sprite = hatSprites[selectedHatIndex];
        leftImage.sprite = hatSprites[(selectedHatIndex - 1 + hatSprites.Length) % hatSprites.Length];
        rightImage.sprite = hatSprites[(selectedHatIndex + 1) % hatSprites.Length];
    }
}
