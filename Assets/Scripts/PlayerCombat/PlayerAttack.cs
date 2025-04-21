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

    private string animatorAttackTriggerName = "attack";
    private string animatorReloadBoolName = "reload";
    private string playerProjectileLayerMaskName = "PlayerProjectile";

    private AudioSource playerAudioSource;

    /// <summary>
    /// Awake called on object is initialised, regardless of whether or not the script is enabled.
    /// </summary>
    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        inventorySwitcher = GetComponent<PlayerInventorySwitcher>();
        playerAudioSource = GetComponent<AudioSource>();
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
        if (weapon.canRunOut)
        {
            if(weapon.currentAmmoCount <= 0)
            {
                return;
            }
        }
        if (weapon.currentAmmoCount <= 0)
        {
            StartCoroutine(ReloadWeapon(weapon));
        }
        switch ((fireInput == 1) ? weapon.firemode1 : weapon.firemode2)
        {
            case PlayerWeapon.firemodes.singleShot:
                shootBullet(weapon, fireInput);
                animateAttack(weapon);
                starterAssetsInputs.shoot = false;
                return;
            case PlayerWeapon.firemodes.burstFire:
                StartCoroutine(BurstFire(weapon, 
                                         fireInput, 
                                         (fireInput == 1) ? weapon.burstAmount1 : weapon.burstAmount2, 
                                         (fireInput == 1) ? weapon.fireRate1 : weapon.fireRate2));
                animateAttack(weapon);
                starterAssetsInputs.shoot = false;
                return;
            case PlayerWeapon.firemodes.fullAuto:
                if (Time.time >= nextFireTime)
                {
                    nextFireTime = Time.time + ((fireInput == 1) ? weapon.fireRate1 : weapon.fireRate2);
                    shootBullet(weapon, fireInput);
                    animateAttack(weapon);
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
        if(weapon.bulletPrefab1.name == "Pez")
        {
            //For Pez we set this gameobject as its parent, so the heal script can quickly add health to the originator of the bullet
            GameObject bullet = Instantiate(((fireInput == 1) ? weapon.bulletPrefab1 : weapon.bulletPrefab2), bulletSpawnTransform);
            bullet.transform.SetParent(this.gameObject.transform);
            weapon.currentAmmoCount--;
        }
        else
        {
            GameObject bullet = Instantiate(((fireInput == 1) ? weapon.bulletPrefab1 : weapon.bulletPrefab2), bulletSpawnTransform);
            bullet.layer = LayerMask.NameToLayer(playerProjectileLayerMaskName);
            bullet.transform.SetParent(null);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * weapon.bulletSpeed1;
            weapon.currentAmmoCount--;
        }
        Debug.Log("attempting to play FX " + weapon.weaponShootFX.name);
        playerAudioSource.PlayOneShot(weapon.weaponShootFX);
    }

    /// <summary>
    /// Check for animation controller, if one is present, then set off the "attack" trigger
    /// </summary>
    /// <param name="weapon"> The reference to the PlayerWeapon object that we are referencing</param>
    private void animateAttack(PlayerWeapon weapon)
    {
        if(weapon.weaponAnimator != null)
        {
            weapon.weaponAnimator.SetTrigger(animatorAttackTriggerName);
        }
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
        ReloadWeaponAnimation(weapon, true);
        if (weapon.weaponReloadFX != null)
        {
            playerAudioSource.PlayOneShot(weapon.weaponReloadFX);
        }
        yield return new WaitForSeconds(weapon.reloadTime);
        weapon.currentAmmoCount = weapon.maxAmmoCount;
        ReloadWeaponAnimation(weapon, false);
        isReloading = false;
    }

    /// <summary>
    /// Check for weapon animator reference, then if found attempt to set reload bool based on input
    /// </summary>
    /// <param name="weapon">PlayerWeapon reference to find the animator on</param>
    /// <param name="input">desired boolean to set reload to</param>
    private void ReloadWeaponAnimation(PlayerWeapon weapon, bool input)
    {
        if(weapon.weaponAnimator != null)
        {
            weapon.weaponAnimator.ResetTrigger(animatorAttackTriggerName);
            weapon.weaponAnimator.SetBool(animatorReloadBoolName, input);
        }
    }
}
