using UnityEngine;

/// <summary>
/// Update Face that is displayed based on health
/// </summary>
public class UIFaceUpdater : MonoBehaviour
{
    public Health playerHealth; // Reference to the player's health script
    public GameObject fullHealthPanel; // Reference to UI panel where full health face is displayed
    public GameObject halfHealthPanel; // Reference to UI panel where half health face is displayed
    public GameObject quarterHealthPanel; // Reference to UI panel where quarter health face is displayed
    public GameObject lowHealthPanel; // Reference to UI panel where low health face is displayed


    void Update()
    {
        if (playerHealth != null && fullHealthPanel != null && halfHealthPanel != null && quarterHealthPanel != null && lowHealthPanel != null)
        {
            if(playerHealth.health <= 10)
            {
                updateFace(3);
                return;
            }
            if(playerHealth.health <= 25)
            {
                updateFace(2);
                return;
            }
            if (playerHealth.health <= 50)
            {
                updateFace(1);
                return;
            }
            else
            {
                updateFace(0);
            }
        }
        else
        {
            Debug.LogError("Health UI element or Player Health not assigned.");
        }
    }

    /// <summary>
    /// Disable all face panels and enable the one we want.
    /// </summary>
    /// <param name="index">index of panel we want</param>
    private void updateFace(int index)
    {
        fullHealthPanel.SetActive(false);
        halfHealthPanel.SetActive(false);
        quarterHealthPanel.SetActive(false);
        lowHealthPanel.SetActive(false);

        switch (index)
        {
            case 0:
                fullHealthPanel.SetActive(true);
                break;
            case 1:
                halfHealthPanel.SetActive(true);
                break;
            case 2:
                quarterHealthPanel.SetActive(true);
                break;
            case 3:
                lowHealthPanel.SetActive(true);
                break;
            default:
                Debug.LogError("Invalid option selected");
                break;
        }
    }
}
