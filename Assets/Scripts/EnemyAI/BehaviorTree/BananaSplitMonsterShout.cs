using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class BananaSplitMonsterShout : Action
{
    public float shoutRadius = 20f; // Radius within which minions will respond to the shout

    public override TaskStatus OnUpdate()
    {
        Vector3 monsterPosition = transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(monsterPosition, shoutRadius);
        bool minionFound = false;

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Minion"))
            {
                SnowconeManController minionController = collider.GetComponent<SnowconeManController>();
                if (minionController != null)
                {
                    minionController.bossCalled(gameObject);
                    minionFound = true;
                }
            }
        }

        if (minionFound)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
