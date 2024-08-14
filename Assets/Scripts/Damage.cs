using System.Collections;
using UnityEngine;

/// <summary>
/// Place on trigger collider objects (like bullets) that should call the damage method from Health class when trigger is entered, then destroy itself.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Damage : MonoBehaviour
{
    [SerializeField] private Health.allDamageType damageType;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private bool doDamageOverTime = false;
    [SerializeField] private int damageTimer = 0;
    private bool isCausingDamageOverTime = false;

    /// <summary>
    /// Turn the collider into a trigger when object is initialised, regardless of whether or not the script is enabled.
    /// </summary>
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    /// <summary>
    /// When something enters this trigger, check the colliding object for the health script, if it is there pass damage amount to damage method, 
    /// Once all script interactions are finished delete the game object this script is attached to.
    /// </summary>
    /// <param name="other"> the Other collider object</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!doDamageOverTime)
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.Damage(damageAmount, damageType);
            }
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("OnTriggerStay");
        if (doDamageOverTime)
        {
            //Debug.Log("doDamageOverTime");
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                //Debug.Log("FoundHealth");
                if (!isCausingDamageOverTime)
                {
                    StartCoroutine(damageWithTime(damageTimer, health));
                }
            }
        }
    }

    private IEnumerator damageWithTime(int seconds, Health health)
    {
        isCausingDamageOverTime = true;
        yield return new WaitForSeconds(seconds);
        //Debug.Log("IEnumerator - damageWithTime");
        health.Damage(damageAmount, damageType);
        isCausingDamageOverTime = false;
    }
}
