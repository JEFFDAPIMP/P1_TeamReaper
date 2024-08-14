using StarterAssets;
using System.Collections;
using UnityEngine;

/// <summary>
/// Handles player attack inputs.
/// </summary>
[RequireComponent(typeof(StarterAssetsInputs))]
[RequireComponent(typeof(PlayerInventorySwitcher))]
public class PlayerAttack : MonoBehaviour
{
    //private serialized local variables
    [SerializeField] private Transform bulletSpawnTransform;

    //private local variables
    private StarterAssetsInputs starterAssetsInputs;
    private PlayerInventorySwitcher inventorySwitcher;
    private PlayerWeapon weapon;

    private float nextFireTime = 0f;

    [SerializeField] private bool isReloading = false;

    /// <summary>
    /// Awake called on object is initialised, regardless of whether or not the script is enabled.
    /// </summary>
    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        inventorySwitcher = GetComponent<PlayerInventorySwitcher>();
    }

    /// <summary>
    /// Update is called once per frame.
    /// checks for player input of shoot or shoot2 then request bullet data from PlayerInventorySwitcher object to instantiate bullet based off current active gun
    /// </summary>
    private void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (starterAssetsInputs.shoot)
        {
            handleShooting(1);
        }

        if (starterAssetsInputs.shoot2)
        {

            handleShooting(2);
        }
    }

    /// <summary>
    /// Based on fire input the player gave and the weapon settings, if there is ammo in the gun, perform shooting, else perform reloading
    /// </summary>
    /// <param name="fireInput">Int - player fire input, 1 or 2</param>
    private void handleShooting(int fireInput)
    {
        weapon = inventorySwitcher.getCurrentWeapon();
        if (weapon.currentAmmoCount <= 0)
        {
            StartCoroutine(ReloadWeapon(weapon));
        }
        switch ((fireInput == 1) ? weapon.firemode1 : weapon.firemode2)
        {
            case PlayerWeapon.firemodes.singleShot:
                shootBullet(weapon, fireInput);
                starterAssetsInputs.shoot = false;
                return;
            case PlayerWeapon.firemodes.burstFire:
                StartCoroutine(BurstFire(weapon, 
                                         fireInput, 
                                         (fireInput == 1) ? weapon.burstAmount1 : weapon.burstAmount2, 
                                         (fireInput == 1) ? weapon.fireRate1 : weapon.fireRate2));
                starterAssetsInputs.shoot = false;
                return;
            case PlayerWeapon.firemodes.fullAuto:
                if (Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + ((fireInput == 1) ? weapon.fireRate1 : weapon.fireRate2);
                    shootBullet(weapon, fireInput);
                }
                return;
            case PlayerWeapon.firemodes.disabled:
                starterAssetsInputs.shoot = false;
                return;
        }
    }

    /// <summary>
    /// Instantiate 1 bullet in the world, and add force based on weapon settings
    /// </summary>
    /// <param name="weapon">Weapon to get the bullet from</param>
    /// <param name="fireInput">player input fire mode</param>
    private void shootBullet(PlayerWeapon weapon, int fireInput)
    {
        GameObject bullet = Instantiate(((fireInput == 1) ? weapon.bulletPrefab1 : weapon.bulletPrefab2), bulletSpawnTransform);
        bullet.transform.SetParent(null);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * weapon.bulletSpeed1;
        weapon.currentAmmoCount--;
    }

    /// <summary>
    /// Perform a burst fire based on weapon player has selected and weapons settings
    /// </summary>
    /// <param name="weapon">weapon player is using</param>
    /// <param name="fireInput">fire input player used</param>
    /// <param name="burstCount">amound of round to fire in succession</param>
    /// <param name="fireRate">how long to wait before firing the next shot</param>
    /// <returns></returns>
    IEnumerator BurstFire(PlayerWeapon weapon, int fireInput, int burstCount, float fireRate)
    {
        for (int i = 0; i < burstCount; i++)
        {
            shootBullet(weapon, fireInput);
            yield return new WaitForSeconds(fireRate);
        }
    }

    /// <summary>
    /// Wait a given amound of time based on weapon reload time and then set weapons ammo count to it's max
    /// </summary>
    /// <param name="weapon">Weapon we want to reload</param>
    /// <returns></returns>
    IEnumerator ReloadWeapon(PlayerWeapon weapon)
    {
        isReloading = true;
        yield return new WaitForSeconds(weapon.reloadTime);
        weapon.currentAmmoCount = weapon.maxAmmoCount;
        isReloading = false;
    }
}
