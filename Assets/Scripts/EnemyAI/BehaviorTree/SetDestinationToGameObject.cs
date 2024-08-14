using UnityEngine;
using UnityEngine.AI;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent
{
    [TaskCategory("Unity/NavMeshAgent")]
    [TaskDescription("Sets the destination of the agent to given GameObject Transform. Returns Success if the destination is valid.")]
    public class SetDestinationToGameObject : Action
    {

        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObjectWithNavMeshAgent;

        [Tooltip("The GameObject that should be navigated towards.")]
        public SharedGameObject targetDistination;

        // cache the navmeshagent component
        private NavMeshAgent navMeshAgent;
        private GameObject prevGameObject;

        /// <summary>
        /// either assume we are trying to access the NavMeshAgent script from ourselves, or from a target object set in targetGameObjectWithNavMeshAgent
        /// </summary>
        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObjectWithNavMeshAgent.Value);
            if (currentGameObject != prevGameObject)
            {
                navMeshAgent = currentGameObject.GetComponent<NavMeshAgent>();
                prevGameObject = currentGameObject;
            }
        }

        /// <summary>
        /// If we could not find a NavMeshAgent, throw warning and return failue
        /// If we found one, try to set new destination, if successful return success else return failure
        /// </summary>
        /// <returns></returns>
        public override TaskStatus OnUpdate()
        {
            if (navMeshAgent == null)
            {
                Debug.LogWarning("NavMeshAgent is null");
                return TaskStatus.Failure;
            }

            return navMeshAgent.SetDestination(targetDistination.Value.transform.position) ? TaskStatus.Success : TaskStatus.Failure;
        }


        /// <summary>
        /// Clear all variables.
        /// </summary>
        public override void OnReset()
        {
            targetGameObjectWithNavMeshAgent = null;
            targetDistination = null;
        }
    }
}
