using UnityEngine;
using TMPro;

/// <summary>
/// Update Health UI to show health
/// </summary>
public class UIHealthUpdater : MonoBehaviour
{
    public Health playerHealth; // Reference to the player's health script
    public TextMeshProUGUI healthText; // Reference to UI text field where health is displayed

    void Update()
    {
        if (playerHealth != null && healthText != null)
        {
            healthText.text = playerHealth.health + "%";
        }
        else
        {
            Debug.LogError("Health UI element or Player Health not assigned.");
        }
    }
}
