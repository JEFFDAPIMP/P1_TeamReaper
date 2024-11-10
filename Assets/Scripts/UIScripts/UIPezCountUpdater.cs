using TMPro;
using UnityEngine;

/// <summary>
/// Update the Pez Ammo Count in the UI
/// </summary>
public class UIPezCountUpdater : MonoBehaviour
{

    public PlayerWeapon pezWeapon; // Reference to the player's health script
    public TextMeshProUGUI pezText; // Reference to UI text field where Pez count is displayed

    // Update is called once per frame
    void Update()
    {
        if (pezWeapon != null && pezText != null)
        {
            pezText.text = pezWeapon.currentAmmoCount.ToString();
        }
        else
        {
            Debug.LogError("pezText UI element or PlayerWeapon for the Pez Gun is not assigned.");
        }
    }
}
