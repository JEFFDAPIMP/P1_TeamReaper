using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles updates to the Arms panel of the UI
/// </summary>
public class UIArmsUpdater : MonoBehaviour
{
    public GameObject weapon1Panel;
    public GameObject weapon2Panel;
    public GameObject weapon3Panel;
    public GameObject weapon4Panel;
    public GameObject weapon5Panel;
    public GameObject weapon6Panel;

    private string playerTag = "Player";
    private string MainCameraTag = "MainCamera";

    public PlayerInventorySwitcher playerInventorySwitcher;
    private int currentIndex;

    /// <summary>
    /// Setup UI to only display weapons in players inventory
    /// </summary>
    private void Start()
    {
        if(playerInventorySwitcher == null)
        {
            playerInventorySwitcher = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerInventorySwitcher>();
        }


        if (weapon1Panel == null || weapon2Panel == null || weapon3Panel == null || weapon4Panel == null || weapon5Panel == null || weapon6Panel == null)
        {
            Debug.LogError("UIArmsUpdater - MISSING WEAPON PANNEL");
        }

        weapon1Panel.SetActive(false);
        weapon2Panel.SetActive(false);
        weapon3Panel.SetActive(false);
        weapon4Panel.SetActive(false);
        weapon5Panel.SetActive(false);
        weapon6Panel.SetActive(false);

        PlayerWeapon[] PlayerWeapons = GameObject.FindGameObjectWithTag(MainCameraTag).GetComponentsInChildren<PlayerWeapon>();
        foreach (PlayerWeapon weapon in PlayerWeapons)
        {
            if (weapon.weaponName == "Pez")
            {
                weapon1Panel.SetActive(true);
            }
            if (weapon.weaponName == "Chomper")
            {
                weapon2Panel.SetActive(true);
            }
            if (weapon.weaponName == "Water Gun")
            {
                weapon3Panel.SetActive(true);
            }
            if (weapon.weaponName == "Bubble Gum Gun")
            {
                weapon4Panel.SetActive(true);
            }
            if (weapon.weaponName == "Icing Gun")
            {
                weapon5Panel.SetActive(true);
            }
            if (weapon.weaponName == "Smores Fork")
            {
                weapon6Panel.SetActive(true);
            }
        }
    }

    
    private void Update()
    {
        changeSelectedWeapon(playerInventorySwitcher.GetCurrentWeaponIndex());
    }

    /// <summary>
    /// if input is different than stored current weapon, update the UI
    /// </summary>
    /// <param name="input"></param>
    private void changeSelectedWeapon(int input)
    {
        if(input == currentIndex)
        {
            return;
        }
        currentIndex = input;
        weapon1Panel.GetComponent<Image>().color = Color.white;
        weapon2Panel.GetComponent<Image>().color = Color.white;
        weapon3Panel.GetComponent<Image>().color = Color.white;
        weapon4Panel.GetComponent<Image>().color = Color.white;
        weapon5Panel.GetComponent<Image>().color = Color.white;
        weapon6Panel.GetComponent<Image>().color = Color.white;

        switch (input)
        {
            case 0:
                weapon1Panel.GetComponent<Image>().color = Color.yellow;
                break;
            case 1:
                weapon2Panel.GetComponent<Image>().color = Color.yellow;
                break;
            case 2:
                weapon3Panel.GetComponent<Image>().color = Color.yellow;
                break;
            case 3:
                weapon4Panel.GetComponent<Image>().color = Color.yellow;
                break;
            case 4:
                weapon5Panel.GetComponent<Image>().color = Color.yellow;
                break;
            case 5:
                weapon6Panel.GetComponent<Image>().color = Color.yellow;
                break;
            default:
                Debug.LogError("UIArmsUpdater - trying to change to an item that doesn't exist");
                break;
        }
    }
}
