using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float playerStamina = 100;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float jumpCost = 20f;
    [HideInInspector] public bool hasRegenerated = true;

    [Header("Stamina Regeneration Settings")]
    [Range(0, 50)] [SerializeField] private float staminaDrain = 0.5f;
    [Range(0, 50)] [SerializeField] private float staminaRegen = 0.5f;

    [Header("Stamina Speed Settings")]
    [SerializeField] private float slowedClimbSpeed = 2f;
    [SerializeField] private float normalClimbSpeed = 5f;

    [Header("Stamina UI Elements")]
    [SerializeField] private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    private WallClimbing_2 wallClimbing;

    private void Start()
    {
        wallClimbing = GetComponent<WallClimbing_2>();
    }

    private void Update()
    {
        if (!wallClimbing.isClimbing)
        {
            RegenerateStamina();
        }
    }

    public void Climbing()
    {
        if (wallClimbing.isClimbing)
        {
            if (playerStamina > 0)
            {
                DrainStamina();
            }
            else
            {
                wallClimbing.climbSpeed = slowedClimbSpeed;
                hasRegenerated = false;
            }
        }
    }

    private void RegenerateStamina()
    {
        if (playerStamina < maxStamina)
        {
            playerStamina += staminaRegen * Time.deltaTime;
            UpdateStamina(1);
        }
        else
        {
            playerStamina = maxStamina;
            wallClimbing.climbSpeed = normalClimbSpeed;
            sliderCanvasGroup.alpha = 0;
            hasRegenerated = true;
        }
    }

    private void DrainStamina()
    {
        playerStamina -= staminaDrain * Time.deltaTime;
        UpdateStamina(1);

        if (playerStamina <= 0)
        {
            playerStamina = 0;
            wallClimbing.climbSpeed = slowedClimbSpeed;
        }
    }


    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;
        if(value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
}
