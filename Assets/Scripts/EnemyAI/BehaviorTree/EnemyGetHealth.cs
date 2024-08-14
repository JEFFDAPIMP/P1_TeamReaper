using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Combat
{
    [TaskDescription("Get Health from Health Script")]
    [TaskCategory("Combat")]
    [TaskIcon("c91b8fe3d68a9114dafd557a82d821d8", "67e27331b399ae14f9eb7a6debc1802d")]
    public class EnemyGetHealth : Action
    {
        [Tooltip("How much health object has")]
        public SharedInt m_health = 1;

        private Health health;
        public override void OnStart()
        {
            health = GetComponent<Health>();
        }

        public override TaskStatus OnUpdate()
        {
            m_health.SetValue(health.GetHealth());
            return TaskStatus.Success;
        }
    }
}
