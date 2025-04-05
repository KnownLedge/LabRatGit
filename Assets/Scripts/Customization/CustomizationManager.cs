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
    [SerializeField] private Image greenTickImage;

    [Header("UI Elements: Objects")]
    [SerializeField] private GameObject leftObject;
    [SerializeField] private GameObject centerObject;
    [SerializeField] private GameObject rightObject;

    [Header("References")]
    [SerializeField] private GameObject customizationMenu;
    [SerializeField] private Ratmovement ratMovement;

    [SerializeField] private Animator cameraAnimator;

    private int selectedHatIndex = 0;
    private int appliedHatIndex = -1;

    private bool playerInZone = false;
    private bool isMenuActive = false;
    private bool isAnimating = false;

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
        // Check if the player is in the zone, key pressed, and animation is not playing
        if (playerInZone && Input.GetKeyDown(KeyCode.E) && !isAnimating)
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
            selectedHatIndex = appliedHatIndex;
            UpdateUI();
            UpdateTickVisibility();

            // Start the opening animation
            cameraAnimator.enabled = true;
            cameraAnimator.SetTrigger("GoToCustomization");
            StartCoroutine(WaitForAnimationToEnd(true)); // Track opening animation
        }
        else
        {
            // Start the closing animation
            cameraAnimator.SetTrigger("GoBackToOriginal");
            StartCoroutine(WaitForAnimationToEnd(false)); // Track closing animation
        }
    }

    // Coroutine to track when animation ends
    private IEnumerator WaitForAnimationToEnd(bool isOpening)
    {
        isAnimating = true; 
        yield return new WaitForSeconds(2);
        isAnimating = false; 

        if (!isOpening)
        {
            cameraAnimator.enabled = false;
        }
    }



    void ChangeSelection(int direction)
    {
        if (hats == null || hats.Length == 0) return;

        selectedHatIndex = (selectedHatIndex + direction + hats.Length) % hats.Length;
        UpdateUI();
        UpdateTickVisibility();
    }

    void ApplyHat()
    {
        appliedHatIndex = selectedHatIndex;
        greenTickImage.gameObject.SetActive(true);
        ActivateSelectedHat();
        PlayerPrefs.SetInt(PLAYER_PREFS_HAT, selectedHatIndex);
        PlayerPrefs.Save();
        UpdateTickVisibility();
    }


    void LoadCustomization()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_HAT))
        {
            selectedHatIndex = PlayerPrefs.GetInt(PLAYER_PREFS_HAT);
            appliedHatIndex = selectedHatIndex;
        }
        ActivateSelectedHat();
        UpdateUI();
        UpdateTickVisibility();
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
        if (hatSprites == null || hatSprites.Length == 0) return;

        centerImage.sprite = hatSprites[selectedHatIndex];
        leftImage.sprite = hatSprites[(selectedHatIndex - 1 + hatSprites.Length) % hatSprites.Length];
        rightImage.sprite = hatSprites[(selectedHatIndex + 1) % hatSprites.Length];
    }

    void UpdateTickVisibility()
    {
        greenTickImage.gameObject.SetActive(false);

        int leftIndex = (selectedHatIndex - 1 + hatSprites.Length) % hatSprites.Length;
        int centerIndex = selectedHatIndex;
        int rightIndex = (selectedHatIndex + 1) % hatSprites.Length;

        if (appliedHatIndex == leftIndex)
        {
            greenTickImage.gameObject.SetActive(true);
            greenTickImage.transform.SetParent(leftImage.transform, true);
        }
        else if (appliedHatIndex == centerIndex)
        {
            greenTickImage.gameObject.SetActive(true);
            greenTickImage.transform.SetParent(centerImage.transform, true);
        }
        else if (appliedHatIndex == rightIndex)
        {
            greenTickImage.gameObject.SetActive(true);
            greenTickImage.transform.SetParent(rightImage.transform, true);
        }

        greenTickImage.rectTransform.localScale = Vector3.one;
        greenTickImage.rectTransform.localScale /= 1.76f;
        greenTickImage.rectTransform.anchoredPosition = new Vector2(0, -101);
    }


}
