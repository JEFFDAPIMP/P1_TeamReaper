using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CheckHealth : Conditional
{
    public SharedInt healthThreshold;
    private Health health;

    public override void OnStart()
    {
        health = GetComponent<Health>();
    }

    public override TaskStatus OnUpdate()
    {
        return health.health <= healthThreshold.Value ? TaskStatus.Success : TaskStatus.Failure;
    }
}
