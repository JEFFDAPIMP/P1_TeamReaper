using StarterAssets;
using System.Collections;
using UnityEngine;

/// <summary>
/// Handles health on any object that should be able to die
/// </summary>
public class Health : MonoBehaviour
{
    //[SerializeField] private int health = 1;
    public int maxHealth = 20;
    public int health = 1;

    enum effectType { Immune, Normal, Vulnerable };
    public enum allDamageType { Crunch, Wet, Burn, Icing, Sticky };


    [SerializeField] effectType chompEffect = effectType.Normal;
    [SerializeField] effectType wetEffect = effectType.Normal;
    [SerializeField] effectType burnEffect = effectType.Normal;
    [SerializeField] effectType icingEffect = effectType.Normal;
    [SerializeField] effectType stickyEffect = effectType.Normal;


    [SerializeField] private int timeToSlowIfIced = 3;
    [SerializeField] private int timeToStopIfSticky = 3;

    [SerializeField] private bool deathHandledByBehaviorTree = false;
    [SerializeField] private bool makeTriggerOnDeath = false;

    public FirstPersonController firstPersonController = null;
    public EnemyBase enemyBase = null;

    private float deathDelay = 0.25f;

    private void Start()
    {
        enemyBase = GetComponent<EnemyBase>();
        firstPersonController = GetComponent<FirstPersonController>();
        health = maxHealth;
    }

    /// <summary>
    /// Method to handle taking damage
    /// </summary>
    /// <param name="damage"> Int - amount to reduce health by</param>
    /// <param name="damageType"> allDamageTypeEnum - Type of damage that is taken</param>
    public void Damage(int damage, allDamageType damageType)
    {
        if (isImmune(damageType))
        {
            return;
        }
        else if (isVulnerable(damageType))
        {
            health -= damage*2;
        }
        else
        {
            health -= damage;
        }
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        if(health < 0)
        {
            health = 0;
        }
        ApplyStatusEffects(damageType);
        StartCoroutine(CheckDeath(deathDelay));
    }

    /// <summary>
    /// Checks if we are immune to given damage type
    /// </summary>
    /// <param name="damageType">Damage type we are checking against</param>
    /// <returns>True if we are immune, False if we are not, False if there is no case found</returns>
    private bool isImmune(allDamageType damageType)
    {
        switch (damageType)
        {
            case allDamageType.Crunch: if (chompEffect == effectType.Immune) { return true; } return false;
            case allDamageType.Wet: if (wetEffect == effectType.Immune) { return true; } return false;
            case allDamageType.Icing: if (icingEffect == effectType.Immune) { return true; } return false;
            case allDamageType.Burn: if (burnEffect == effectType.Immune) { return true; } return false;
            case allDamageType.Sticky: if (stickyEffect == effectType.Immune) { return true; } return false;
            default : return false;
        }
    }

    /// <summary>
    /// Check if we are Vulnerable to given damage type
    /// </summary>
    /// <param name="damageType">Damage type we are checking against</param>
    /// <returns>True if we are Vulnerable, False if we are not, False if there is no case found</returns>
    private bool isVulnerable(allDamageType damageType)
    {
        switch (damageType)
        {
            case allDamageType.Crunch: if (chompEffect == effectType.Vulnerable) { return true; } return false;
            case allDamageType.Wet: if (wetEffect == effectType.Vulnerable) { return true; } return false;
            case allDamageType.Icing: if (icingEffect == effectType.Vulnerable) { return true; } return false;
            case allDamageType.Burn: if (burnEffect == effectType.Vulnerable) { return true; } return false;
            case allDamageType.Sticky: if (stickyEffect == effectType.Vulnerable) { return true; } return false;
            default: return false;
        }
    }

    /// <summary>
    /// Apply Status effects to player or enemy
    /// </summary>
    /// <param name="damageType">Damage Type to apply status effect for</param>
    private void ApplyStatusEffects(allDamageType damageType)
    {
        //Debug.Log("ApplyStatusEffects");
        if (damageType == allDamageType.Icing)
        {
            //Debug.Log("ApplyStatusEffects - Icing");
            if (firstPersonController != null)
            {
                //Debug.Log("firstPersonController Found");
                StartCoroutine(slowPlayerForSeconds(timeToSlowIfIced));
            }
            else if (enemyBase != null)
            {
                StartCoroutine(slowEnemyForSeconds(timeToSlowIfIced));
            }
        }
    }

    /// <summary>
    /// tell first person controller that we are iced to slow down
    /// </summary>
    /// <param name="seconds">Int - number of seconds to be iced for</param>
    /// <returns>WaitForSeconds(int seconds)</returns>
    private IEnumerator slowPlayerForSeconds(int seconds)
    {
        //Debug.Log("Inside slow player for time");
        firstPersonController.iced = true;
        yield return new WaitForSeconds(seconds);
        firstPersonController.iced = false;
    }

    /// <summary>
    /// tell enemy controller that we are iced to slow down
    /// </summary>
    /// <param name="seconds">Int - number of seconds to be iced for</param>
    /// <returns>WaitForSeconds(int seconds)</returns>
    private IEnumerator slowEnemyForSeconds(int seconds)
    {
        //Debug.Log("Inside slow enemy for time");
        enemyBase.iced = true;
        yield return new WaitForSeconds(seconds);
        enemyBase.iced = false;
    }

    //
    /// <summary>
    /// Used to check if you have no health, if health is less than 1, you are dead and we set gameobject to an inactive state
    /// </summary>
    private IEnumerator CheckDeath(float seconds)
    {
        if (!deathHandledByBehaviorTree)
        {
            if (health < 1)
            {
                yield return new WaitForSeconds(seconds);
                if (makeTriggerOnDeath)
                {
                    this.gameObject.GetComponent<Collider>().isTrigger = true;
                }
                else
                {
                    if (firstPersonController == null && enemyBase == null)
                    {
                        try
                        {
                            Destroy(this.gameObject);
                        }
                        catch
                        {
                            Debug.LogError("Trying to delete something that is not there");
                        }

                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Public accessor to get health of game object
    /// </summary>
    /// <returns>Int - current health</returns>
    public int GetHealth()
    {
        return this.health;
    }
}
