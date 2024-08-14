using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class is used for testing simple A-Star Pathing
/// </summary>
public class SimpleNavMeshTest : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.SetDestination(target.position);
    }

}
