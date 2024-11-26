using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float playerStamina = 100;
    [SerializeField] private float maxStamina = 100f;
    public float jumpCost = 20f;
    [HideInInspector] public bool hasRegenerated = true;

    [Header("Stamina Regeneration Settings")]
    [Range(0, 50)][SerializeField] private float staminaDrain = 0.5f;
    [Range(0, 50)][SerializeField] private float staminaRegen = 0.5f;

    [Header("Stamina Speed Settings")]
    [SerializeField] private float slowedClimbSpeed = 2f;
    [SerializeField] private float normalClimbSpeed = 5f;

    [Header("Stamina UI Elements")]
    [SerializeField] private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    private WallClimbing_2 wallClimbing;
    private LedgeClimbing_2 ledgeClimbing;

    private void Start()
    {
        // Get the required components
        wallClimbing = GetComponent<WallClimbing_2>();
        ledgeClimbing = GetComponent<LedgeClimbing_2>();

        // Log an error if components are not assigned
        if (wallClimbing == null)
        {
            Debug.LogError("WallClimbing_2 component is missing!");
        }
        if (ledgeClimbing == null)
        {
            Debug.LogError("LedgeClimbing_2 component is missing!");
        }
    }

    private void Update()
    {
        // Regenerate stamina if not climbing
        if (!wallClimbing.isClimbing)
        {
            RegenerateStamina();
        }
    }

    public void Climbing()
    {
        // Check if we are climbing or sticking to a ledge
        if (wallClimbing.isClimbing || ledgeClimbing.isStickingToLedge)
        {
            if (playerStamina > 0)
            {
                DrainStamina();
            }
            else
            {
                // Set stamina regeneration flag to false when stamina is drained
                hasRegenerated = false;
            }
        }
    }

    private void RegenerateStamina()
    {
        // Regenerate stamina if it's less than max stamina
        if (playerStamina < maxStamina)
        {
            playerStamina += staminaRegen * Time.deltaTime;
            UpdateStamina(1);
        }
        else
        {
            playerStamina = maxStamina;
            sliderCanvasGroup.alpha = 0;
            hasRegenerated = true;
        }
    }

    private void DrainStamina()
    {
        // If wallClimbing and ledgeClimbing components are not null, drain stamina
        if (wallClimbing != null && ledgeClimbing != null)
        {
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);

            if (playerStamina <= 0)
            {
                playerStamina = 0;

                // Stop climbing and ledge sticking
                wallClimbing.isClimbing = false;
                ledgeClimbing.isStickingToLedge = false;

                // If ledgeClimbing is valid, call FallFromLedge
                if (ledgeClimbing != null)
                {
                    Debug.Log("Calling FallFromLedge");
                    ledgeClimbing.FallFromLedge();
                }
                else
                {
                    Debug.LogWarning("LedgeClimbing_2 is null, unable to call FallFromLedge.");
                }

                // Stop climbing
                wallClimbing.StopClimbing();
            }
        }
        else
        {
            // Log error if components are missing
            if (wallClimbing == null)
            {
                Debug.LogError("WallClimbing_2 is not assigned!");
            }

            if (ledgeClimbing == null)
            {
                Debug.LogError("LedgeClimbing_2 is not assigned!");
            }
        }
    }

    public void UpdateStamina(int value)
    {
        // Update the stamina UI
        staminaProgressUI.fillAmount = playerStamina / maxStamina;

        // Show or hide the stamina slider UI
        if (value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
}
