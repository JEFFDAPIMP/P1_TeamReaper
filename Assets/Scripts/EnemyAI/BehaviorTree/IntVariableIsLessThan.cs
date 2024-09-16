using UnityEngine;
using System;
using System.Reflection;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Check if int set in variables is less that target.")]
    [TaskCategory("Reflection")]
    [TaskIcon("{SkinColor}ReflectionIcon.png")]
    public class IntVariableIsLessThan : Conditional
    {
        [Tooltip("Variable that should be less")]
        public SharedInt variableToCheck;
        [Tooltip("Value that variable should be less than")]
        public SharedInt valueToBeLessThan;

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