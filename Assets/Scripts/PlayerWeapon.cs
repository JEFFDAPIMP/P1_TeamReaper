using UnityEngine;

/// <summary>
/// This class hold references to data to be used by the PlayerAttack.cs script
/// </summary>
public class PlayerWeapon : MonoBehaviour
{
    public enum firemodes { singleShot, burstFire, fullAuto, disabled };

    [Header("Weapon values for fire input 1")]
    public firemodes firemode1 = firemodes.singleShot;
    public GameObject bulletPrefab1;
    public float bulletSpeed1;
    public float fireRate1;
    public int burstAmount1;

    [Header("Weapon values for fire input 2")]
    public firemodes firemode2 = firemodes.disabled;
    public GameObject bulletPrefab2;
    public float bulletSpeed2;
    public float fireRate2;
    public int burstAmount2;

    [Header("Weapon values for ammo count and reload speed")]
    public int maxAmmoCount;
    public int currentAmmoCount;
    public float reloadTime;
}
