using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using System.Collections;

public class MoveToCover : Action
{
    public SharedTransform coverPosition; // Position of the cover
    private NavMeshAgent agent;
    private bool destinationSet = false;

    public override void OnStart()
    {
        //Debug.Log("Task started: " + this.GetType().Name);
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(SetDestinationAndWait());
    }

    IEnumerator SetDestinationAndWait()
    {
        agent.SetDestination(coverPosition.Value.position);
        //yield return new WaitUntil(() => agent.pathPending == false); // Wait until the path is calculated
        //agent.nextPosition = coverPosition.Value.position;
        //agent.ResetPath();
        yield return new WaitForSeconds(1f);
        destinationSet = true;
    }

    public override TaskStatus OnUpdate()
    {
        //Debug.Log("Task updated: " + this.GetType().Name);
        //Debug.Log("MoveToCover  agent.remainingDistance = " + agent.remainingDistance);
        if (destinationSet && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.ResetPath();
            if (destinationSet && agent.remainingDistance <= agent.stoppingDistance)
            {
                //Debug.Log("MoveToCover - Success");
                return TaskStatus.Success;
            }
        }
        //Debug.Log("MoveToCover - Running");
        return TaskStatus.Running;
    }
}
