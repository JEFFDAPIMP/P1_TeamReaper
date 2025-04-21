using UnityEngine;

public class HealParent : MonoBehaviour
{
    [SerializeField] private int healAmount = 1;

    // When spawned, get parent health componment, "damage" it for negative heal amount, then delete this game object.
    void Start()
    {
        this.gameObject.GetComponentInParent<Health>().Damage((healAmount * -1), Health.allDamageType.Crunch);
        this.gameObject.GetComponentInParent<Health>().playHealSoundFX();
        Destroy(this.gameObject);
    }
}
