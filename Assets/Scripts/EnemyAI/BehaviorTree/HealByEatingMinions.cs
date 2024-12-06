using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class HealByEatingMinions : Action
{
    public int healAmount = 10;
    public string minionTag = "Minion";
    public float radius = 2f;

    //Kill all minions in radius and heal for each one killed
    public override TaskStatus OnUpdate()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(minionTag))
            {
                hitCollider.GetComponent<Health>().Damage(1000, Health.allDamageType.Crunch);
                // Increase the boss's health
                this.gameObject.GetComponent<Health>().Damage(-healAmount, Health.allDamageType.Wet);
            }
        }
        return TaskStatus.Success;
    }
}
