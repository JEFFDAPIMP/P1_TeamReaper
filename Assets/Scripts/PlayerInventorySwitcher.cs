using StarterAssets;
using UnityEngine;

/// <summary>
/// This Class handles switching between items in inventory
/// You will use middle mouse wheel or the "switch item" button to cycle though different items in your inventory
/// </summary>
[RequireComponent(typeof(StarterAssetsInputs))]
public class PlayerInventorySwitcher : MonoBehaviour
{
    //array of player items, and starting index
    [SerializeField] private GameObject[] playerInventory;
    [SerializeField] private int currentItemIndex = 0;

    //private local variables
    private StarterAssetsInputs starterAssetsInputs;

    /// <summary>
    /// Awake called on object is initialised, regardless of whether or not the script is enabled.
    /// </summary>
    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    /// <summary>
    /// Start is called before the first frame update
    /// Set all items in inventory to inactive state then set first item in inventory to active.
    /// </summary>
    private void Start()
    {
        setActiveItemInHand(currentItemIndex);
    }

    /// <summary>
    /// Update is called once per frame
    /// Check to see if player scrolled up, or scrolled down, or clicked the "switch item" button then change active item accordingly.
    /// </summary>
    private void Update()
    {
        //Change Weapon Back
        if(starterAssetsInputs.changeWeaponBack || starterAssetsInputs.scrollDirection.y > Vector2.up.y)
        {
            //Set current item index either back 1 or at the other end of the array
            if(currentItemIndex == 0)
            {
                currentItemIndex = playerInventory.Length - 1;
            }
            else
            {
                currentItemIndex--;
            }

            setActiveItemInHand(currentItemIndex);
            starterAssetsInputs.changeWeaponBack = false;
        }

        //Change Weapon Forward
        if (starterAssetsInputs.changeWeaponForward || starterAssetsInputs.scrollDirection.y < Vector2.down.y)
        {
            //Set current item index either forward 1 or at beging of the array
            if (currentItemIndex < playerInventory.Length-1)
            {
                currentItemIndex++;
            }
            else
            {
                currentItemIndex = 0;
            }

            setActiveItemInHand(currentItemIndex);
            starterAssetsInputs.changeWeaponForward = false;
        }
        
    }


    private void setActiveItemInHand(int index)
    {
        //Set every item in player inventory to active false
        foreach (GameObject item in playerInventory)
        {
            item.SetActive(false);
        }

        //turn on target item in player inventory
        playerInventory[index].SetActive(true);
    }

    public PlayerWeapon getCurrentWeapon()
    {
        return playerInventory[currentItemIndex].GetComponent<PlayerWeapon>();
    }

    public int GetCurrentWeaponIndex()
    {
        return currentItemIndex;
    }
}
