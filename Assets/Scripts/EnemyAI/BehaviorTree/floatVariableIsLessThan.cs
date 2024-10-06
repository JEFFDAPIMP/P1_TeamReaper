using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Check if int set in variables is less that target.")]
    [TaskCategory("Reflection")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class floatVariableIsLessThan : Conditional
    {
        [Tooltip("Variable that should be less")]
        public SharedFloat variableToCheck;
        [Tooltip("Value that variable should be less than")]
        public SharedFloat valueToBeLessThan;

        public override TaskStatus OnUpdate()
        {
            if (variableToCheck == null)
            {
                Debug.LogWarning("Unable to compare field - variableToCheck is null");
                return TaskStatus.Failure;
            }

            if (valueToBeLessThan == null)
            {
                Debug.LogWarning("Unable to compare field - valueToBeLessThan null");
                return TaskStatus.Failure;
            }

            return (variableToCheck.Value < valueToBeLessThan.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            variableToCheck = null;
            valueToBeLessThan = null;
        }
    }
}