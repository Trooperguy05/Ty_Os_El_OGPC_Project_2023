using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    [Header("Stamina UI")]
    public Slider staminaSlider;

    [Header("Stamina Calcs")]
    public float maxStamina;
    public float staminaConsumeRate;
    public float staminaRechargeRate;
    public float staminaRechargeDelay;
    public float currentStamina;

    [Header("Test Vars")]
    public bool isDepleting = false;
    public bool isRegenerating = false;

    [Header("PlayerSFX")]
    private PlayerSFX playerSFX;

    // set values on slider
    void Awake()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
        playerSFX = GetComponent<PlayerSFX>();
    }

    // method that depletes stamina
    public IEnumerator depleteStamina() {
        // if the player has stamina
        if (currentStamina > 0) {
            isDepleting = true;
            while (currentStamina > 0) {
                currentStamina -= staminaConsumeRate;
                updateStaminaUI();
                // Start the heavy breathing FMOD event \\
                if (currentStamina < 30) playerSFX.startHeavyBreathing(); 
                yield return new WaitForSeconds(0.1f);

            }
            isDepleting = false;

            // if all stamina is gone, start regenning health
            if (currentStamina <= 0) StartCoroutine(regenStamina());
        }
    }

    // method that regens stamina
    public IEnumerator regenStamina() {
        isRegenerating = true;
        yield return new WaitForSeconds(staminaRechargeDelay);

        while (currentStamina < maxStamina) {
            currentStamina += staminaRechargeRate;
            updateStaminaUI();
            // Set the "Stamina" FMOD Event parameter to current stamina so FMOD knows when to stop event \\
            playerSFX.heavyBreathingEmitter.SetParameter("Stamina", currentStamina); 
            yield return new WaitForSeconds(0.1f);
        }
        isRegenerating = false;
    }

    // method that updates the UI, but makes sure that current stamina is 0 < x < maxStamina
    private void updateStaminaUI() {
        if (currentStamina > maxStamina) currentStamina = maxStamina;
        else if (currentStamina < 0) currentStamina = 0;

        staminaSlider.value = currentStamina;
    }

    // method that stops the depleting stamina and starts regenning
    public void stopRunning() {
        StopAllCoroutines();
        isDepleting = false;
        StartCoroutine(regenStamina());
    }
    // method that starts depleting stamina and stops regenning
    public void startRunning() {
        StopAllCoroutines();
        isRegenerating = false;
        StartCoroutine(depleteStamina());
    }
}
