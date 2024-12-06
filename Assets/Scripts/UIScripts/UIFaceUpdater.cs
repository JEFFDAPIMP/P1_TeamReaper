using UnityEngine;

/// <summary>
/// Update Face that is displayed based on health
/// </summary>
public class UIFaceUpdater : MonoBehaviour
{
    public Health playerHealth; // Reference to the player's health script
    public GameObject fullHealthPanel; // Reference to UI panel where full health face is displayed
    public GameObject hit1; // Reference to UI panel where half health face is displayed
    public GameObject hit2; // Reference to UI panel where quarter health face is displayed
    public GameObject hit3; // Reference to UI panel where low health face is displayed
    public GameObject hit4; // Reference to UI panel where low health face is displayed
    public GameObject hit5_dead; // Reference to UI panel where low health face is displayed


    void Update()
    {
        if (playerHealth != null && fullHealthPanel != null && hit1 != null && hit2 != null && hit4 != null)
        {
            if(playerHealth.health <= 1)
            {
                updateFace(5);
                return;
            }
            if(playerHealth.health <= 20)
            {
                updateFace(4);
                return;
            }
            if (playerHealth.health <= 40)
            {
                updateFace(3);
                return;
            }
            if (playerHealth.health <= 60)
            {
                updateFace(2);
                return;
            }
            if (playerHealth.health <= 80)
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
        hit1.SetActive(false);
        hit2.SetActive(false);
        hit3.SetActive(false);
        hit4.SetActive(false);
        hit5_dead.SetActive(false);

        switch (index)
        {
            case 0:
                fullHealthPanel.SetActive(true);
                break;
            case 1:
                hit1.SetActive(true);
                break;
            case 2:
                hit2.SetActive(true);
                break;
            case 3:
                hit3.SetActive(true);
                break;
            case 4:
                hit4.SetActive(true);
                break;
            case 5:
                hit5_dead.SetActive(true);
                break;
            default:
                Debug.LogError("Invalid option selected");
                break;
        }
    }
}
