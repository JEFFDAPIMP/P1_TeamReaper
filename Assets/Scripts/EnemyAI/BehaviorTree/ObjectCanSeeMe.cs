using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    /// <summary>
    /// Returns true if we can raycast to object
    /// </summary>
    public class CanRaycastToObject : Conditional
    {
        [Tooltip("The Target Object we want to check if can see us")]
        [UnityEngine.Serialization.FormerlySerializedAs("targetObjectToCheckFor")]
        public SharedGameObject ObjectToCheckFor;

        [Tooltip("The target raycast offset relative to the pivot position.")]
        [UnityEngine.Serialization.FormerlySerializedAs("targetOffset")]
        public SharedVector3 m_TargetOffset;

        public override TaskStatus OnUpdate()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (ObjectToCheckFor.Value.transform.position + m_TargetOffset.Value), out hit))
            {

            }
            return TaskStatus.Success;
        }
    }
}
