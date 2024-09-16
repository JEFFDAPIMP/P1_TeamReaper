using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

public class MoveToCover : Action
{
    public SharedTransform coverPosition; // Position of the cover
    private NavMeshAgent agent;

    public override void OnStart()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(coverPosition.Value.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            return TaskStatus.Success;
        }
        Debug.Log("MoveToCover - Running");
        return TaskStatus.Running;
    }
}
