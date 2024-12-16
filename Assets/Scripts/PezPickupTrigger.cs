using UnityEngine;

/// <summary>
/// Handle player picking up Pez.
/// If the player doesn't have max pez then pick up else ignore.
/// </summary>
public class PezPickupTrigger : MonoBehaviour
{
    public PlayerWeapon pezWeapon;

    //Shouldn't need to change these.
    private string playerTag = "Player";
    private string MainCameraTag = "MainCamera";
    private string expectedPezWeaponName = "Pez Dispensor";


    /// <summary>
    /// If the players Pez Weapon is not assigned search for it
    /// </summary>
    private void Start()
    {
        if (pezWeapon != null)
        {
            return;
        }

        PlayerWeapon[] PlayerWeapons = GameObject.FindGameObjectWithTag(MainCameraTag).GetComponentsInChildren<PlayerWeapon>();
        foreach (PlayerWeapon weapon in PlayerWeapons)
        {
            if (weapon.weaponName == expectedPezWeaponName)
            {
                pezWeapon = weapon;
                break;
            }
        }

        if (pezWeapon == null)
        {
            Debug.Log("Pez weapon not set, check expectedPezWeaponName of PezPickupTrigger, to the set Weapon Name of the Pez weapon");
        }
    }

    /// <summary>
    /// If player walks into this object and has room for more pez, run PickUp()
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == playerTag)
        {
            if(pezWeapon.currentAmmoCount < pezWeapon.maxAmmoCount)
            {
                PickUp();
            }
        }
    }

    /// <summary>
    /// Increase player's Pez count then delete self
    /// </summary>
    private void PickUp()
    {
        pezWeapon.currentAmmoCount++;
        Destroy(this.gameObject);
    }
}
