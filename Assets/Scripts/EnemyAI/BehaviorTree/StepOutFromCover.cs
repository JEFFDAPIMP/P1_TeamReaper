using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using System.Collections;

public class StepOutFromCover : Action
{
    public SharedTransform stepOutPosition; // Position to step out to
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
        agent.SetDestination(stepOutPosition.Value.position);
        //agent.nextPosition = stepOutPosition.Value.position;
        //agent.ResetPath();
        //Debug.Log("StepOutFromCover  agent.remainingDistance = " + agent.remainingDistance);
        //yield return new WaitUntil(() => agent.pathPending == false); // Wait until the path is calculated
        yield return new WaitForSeconds(1f);
        //Debug.Log("StepOutFromCover  agent.remainingDistance = " + agent.remainingDistance);
        destinationSet = true;
    }

    public override TaskStatus OnUpdate()
    {
        //Debug.Log("Task updated: " + this.GetType().Name);
        //Debug.Log("StepOutFromCover  agent.remainingDistance = " + agent.remainingDistance);

        if (destinationSet && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.ResetPath();
            if (destinationSet && agent.remainingDistance <= agent.stoppingDistance)
            {
                //Debug.Log("StepOutFromCover - Success");
                return TaskStatus.Success;
            }
        }
        //Debug.Log("StepOutFromCover - Running");
        return TaskStatus.Running;
    }
}
