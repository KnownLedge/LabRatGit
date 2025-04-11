using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CustomizationManager : MonoBehaviour
{
    private const string PLAYER_PREFS_HAT = "SelectedHat";

    [Header("Hat Customization")]
    [SerializeField] private GameObject[] hats; // Array of hat prefabs
    [SerializeField] private Sprite[] hatSprites; // Hat sprites for UI buttons

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
    [SerializeField] private GameObject textObject;

    [Header("References")]
    [SerializeField] private GameObject segmentedPlayer;
    [SerializeField] private GameObject player; 
    [SerializeField] private GameObject customizationMenu;
    [SerializeField] private Ratmovement ratMovement;
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private Shader shader;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private AudioClip buttonSound;

    [Header("HeadPosReferences")]
    [SerializeField] private Transform chefHatHead;
    [SerializeField] private Transform cowboyHatHead;
    [SerializeField] private Transform flowerHatHead;
    [SerializeField] private Transform partyHatHead;
    [SerializeField] private Transform propellerHatHead;
    [SerializeField] private Transform strawberryHatHead;
    [SerializeField] private Transform topHatHead;
    [SerializeField] private Transform wizardHatHead;

    [SerializeField] private float rotationSpeed = 2f;


    private int selectedHatIndex = 0;
    private int appliedHatIndex = 0;

    private bool playerInZone = false;
    private bool isMenuActive = false;
    private bool isAnimating = false;

    private GameObject currentHat; // Variable to keep track of the currently applied hat

    void Awake()
    {
        DontDestroyOnLoad(gameObject); 

        leftButton.onClick.AddListener(() => ChangeSelection(-1)); // Left button to change selection to previous hat
        rightButton.onClick.AddListener(() => ChangeSelection(1)); // Right button to change selection to next hat
        applyButton.onClick.AddListener(ApplyHat); // Apply button to save the selected hat

        UpdateUI();
        UpdateTickVisibility();
        customizationMenu.SetActive(false); // Ensure the menu is inactive at start
        textObject.SetActive(false); // Ensure the text object is inactive at start

        shader = player.GetComponent<Shader>();//get the shader from the player game object
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMenuActive)
        {
            Debug.Log("Player entered customization zone.");
            playerInZone = true;
            textObject.SetActive(true);
            Debug.Log("Shader enabled: " + shader.enabled);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isMenuActive)
        {
            Debug.Log("Player exited customization zone.");
            playerInZone = false;
            textObject.SetActive(false);
            Debug.Log("Shader enabled: " + shader.enabled);
        }
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E) && !isAnimating)
        {
            ToggleMenu(!isMenuActive);
            
        }

        if (isMenuActive)
        {
            // Allow player rotation only when RMB is held down
            if (Input.GetMouseButton(1))  // Right Mouse Button (1)
            {
                float rotation = Input.GetAxis("Mouse X") * rotationSpeed;
                player.transform.Rotate(Vector3.up, rotation, Space.World);
            }
        }
    }
    void ToggleMenu(bool state)
    {
        isMenuActive = state;
        customizationMenu.SetActive(state);

        if (ratMovement != null)
        {
            ratMovement.enabled = false; // Disable player movement while the menu is active
        }

        if (state)
        {
            textObject.SetActive(false);
            Debug.Log("TextObject: " + textObject.activeSelf);
            selectedHatIndex = appliedHatIndex; // Ensure the selected hat index is set to the applied hat index when opening the menu
            UpdateUI();
            UpdateTickVisibility();

            // Start the opening animation
            cameraAnimator.enabled = true;
            cameraAnimator.SetTrigger("GoToCustomization");
            StartCoroutine(WaitForAnimationToEnd(true)); // Track opening animation

            // Ensure no other scripts are modifying position or rotation
            player.GetComponent<Rigidbody>().isKinematic = true;

            // Set position and rotation
            player.transform.localPosition = new Vector3(-15.8299999f, -0.949999988f, -1.13f);
            player.transform.localRotation = Quaternion.Euler(0, 100, 0);

           
            Destroy(shader);//remove the shader
        }
        else
        {
            player.GetComponent<Rigidbody>().isKinematic = false;

            // Start the closing animation
            cameraAnimator.SetTrigger("GoBackToOriginal");
            StartCoroutine(WaitForAnimationToEnd(false)); // Track closing animation
            shader = player.AddComponent<Shader>();//re-add the shader
        }
    }

    // Coroutine to track when animation ends
    private IEnumerator WaitForAnimationToEnd(bool isOpening)
    {
        isAnimating = true; 
        yield return new WaitForSeconds(2);  // Wait for animation to end
        isAnimating = false; 

        if (!isOpening)
        {
            cameraAnimator.enabled = false;
            if (ratMovement != null)
            {
                ratMovement.enabled = true; // Disable player movement while the menu is active
            }
        }
        
        Debug.Log("Player position: " + player.transform.localPosition);
        Debug.Log("Player rotation: " + player.transform.localRotation);
    }

    void ChangeSelection(int direction)
    {
        if (hats == null || hats.Length == 0) return;

        // Change the selected hat index based on the button press (left or right)
        selectedHatIndex = (selectedHatIndex + direction + hats.Length) % hats.Length;

        // Immediately update the hat and UI
        ActivateSelectedHat();
        UpdateUI();
        UpdateTickVisibility();
        if (buttonSound != null)
        {
            audioManager.PlaySFX(buttonSound);
        }
    }

    void ApplyHat()
    {
        appliedHatIndex = selectedHatIndex; // Save the selected hat as the applied one
        greenTickImage.gameObject.SetActive(true); // Show the green tick image on the applied hat
        PlayerPrefs.SetInt(PLAYER_PREFS_HAT, selectedHatIndex); // Save the applied hat index to PlayerPrefs
        PlayerPrefs.Save();
        UpdateTickVisibility();
        if (buttonSound != null)
        {
            audioManager.PlaySFX(buttonSound);
        }
    }


    void ActivateSelectedHat()
    {
        if (currentHat != null)
        {
            Destroy(currentHat);
            currentHat = null;
        }

        if (selectedHatIndex >= 0 && selectedHatIndex < hats.Length)
        {
            GameObject hatPrefab = hats[selectedHatIndex];

            if (hatPrefab == null)
            {
                Debug.Log("No hat selected. Player will wear no hat.");
                return; // Do nothing, leave head empty
            }

            Transform targetHead = GetHatTargetTransform(hatPrefab.name);
            if (targetHead == null)
            {
                Debug.LogWarning($"No head reference found for hat: {hatPrefab.name}");
                return;
            }

            currentHat = Instantiate(hatPrefab, targetHead);
            currentHat.transform.localPosition = Vector3.zero;
            currentHat.transform.localRotation = Quaternion.identity;
            currentHat.transform.localScale = Vector3.one;
            currentHat.SetActive(true);
        }
    }

    Transform GetHatTargetTransform(string hatName)
    {
        hatName = hatName.ToLower();

        if (hatName.Contains("cowboy")) return cowboyHatHead;
        if (hatName.Contains("party")) return partyHatHead;
        if (hatName.Contains("chef")) return chefHatHead;
        if (hatName.Contains("flower")) return flowerHatHead;
        if (hatName.Contains("propeller")) return propellerHatHead;
        if (hatName.Contains("strawberry")) return strawberryHatHead;
        if (hatName.Contains("top")) return topHatHead;
        if (hatName.Contains("wizard")) return wizardHatHead;

        return null;
    }

    void UpdateUI()
    {
        if (hatSprites == null || hatSprites.Length == 0) return;
        if (selectedHatIndex < 0 || selectedHatIndex >= hatSprites.Length)
        {
            Debug.LogError($"Selected hat index {selectedHatIndex} is out of bounds. Hat sprites length: {hatSprites.Length}");
            return;
        }


        // Update UI images based on the selected hat
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

        // Show the green tick image on the applied hat's UI icon
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

        // Adjust the green tick position and scale
        greenTickImage.rectTransform.localScale = Vector3.one;
        greenTickImage.rectTransform.localScale /= 1.76f;
        greenTickImage.rectTransform.anchoredPosition = new Vector2(0, -101);
    }
}
