using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityIcons : MonoBehaviour
{
    [SerializeField] private GameObject HealAbilityOn;
    [SerializeField] private GameObject HealAbilityOff;
    [SerializeField] private GameObject UltimateAbilityOn;
    [SerializeField] private GameObject UltimateAbilityOff;

    private GameManager gameManager;

    void Start() {
        gameManager = GameManager.Instance;
        
        if (!gameManager.unlockedAbilityIcons) {
            DisableAllIcons();
        }
    }

    void Update()
    {
        if (gameManager.unlockedAbilityIcons) {
            EnableAllIcons();
        } else {
            return;
        }
        
        if (gameManager.hasQi) {
            ActivateHealAbilityIcon();
        } else {
            DeactivateHealAbilityIcon();
        }

        if (gameManager.unlockedUltimate) {
            if (GameManager.Instance.hasMaxQi) {
                ActivateUltimateAbilityIcon();
            } else {
                DeactivateUltimateAbilityIcon();
            }
        }
    }

    private void ActivateHealAbilityIcon()
    {
        HealAbilityOff.SetActive(false);
        HealAbilityOn.SetActive(true);
    }
    private void DeactivateHealAbilityIcon()
    {
        HealAbilityOff.SetActive(true);
        HealAbilityOn.SetActive(false);
    }
    private void ActivateUltimateAbilityIcon()
    {
        UltimateAbilityOff.SetActive(false);
        UltimateAbilityOn.SetActive(true);
    }
    private void DeactivateUltimateAbilityIcon()
    {
        UltimateAbilityOff.SetActive(true);
        UltimateAbilityOn.SetActive(false);
    }

    private void DisableAllIcons() {
        HealAbilityOff.SetActive(false);
        UltimateAbilityOff.SetActive(false);
        HealAbilityOn.SetActive(false);
        UltimateAbilityOn.SetActive(false);
    }
    private void EnableAllIcons() {
        HealAbilityOff.SetActive(true);
        UltimateAbilityOff.SetActive(true);
        // HealAbilityOn.SetActive(true);
        // UltimateAbilityOn.SetActive(true);
    }
}
