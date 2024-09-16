using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class StepOutFromCover : Action
{
    public SharedTransform stepOutPosition; // Position to step out to
    private NavMeshAgent agent;

    public override void OnStart()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(stepOutPosition.Value.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            return TaskStatus.Success;
        }
        Debug.Log("StepOutFromCover - Running");
        return TaskStatus.Running;
    }
}
