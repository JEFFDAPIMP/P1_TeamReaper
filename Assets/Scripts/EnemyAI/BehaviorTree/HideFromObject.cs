using UnityEngine;
using UnityEngine.AI;


namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Find furthest hiding spot from given object")]
    [TaskCategory("Movement")]
    [TaskIcon("c91b8fe3d68a9114dafd557a82d821d8", "67e27331b399ae14f9eb7a6debc1802d")]
    public class HideFromObject : Action
    {
        private NavMeshAgent agent;

        public SharedGameObject Player;
        public VisionCheck[] HidingPlaces;

        public SharedTransform myTargetHidingPlace;
        private float distanceFromPlayer = 0;

        public override void OnStart()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public override TaskStatus OnUpdate()
        {
            if (!myTargetHidingPlace.Value)
            {
                setHidingPlace();
            }
            if (myTargetHidingPlace.Value.GetComponent<VisionCheck>().CanSeePlayer())
            {
                setHidingPlace();
            }
            return TaskStatus.Success;
        }

        private void setHidingPlace()
        {
            foreach (VisionCheck hidingPlace in HidingPlaces)
            {

                //If my hiding place is hidden from the player
                if (!hidingPlace.CanSeePlayer())
                {
                    Debug.Log("Found Hiding place hidden from player");
                    //if I already have a target hiding place, check to see if the otherone is closer
                    if (myTargetHidingPlace.Value)
                    {
                        Debug.Log("I have a hiding place, Checking to see if its better");
                        //if (distanceToHidingPlace > Vector3.Distance(hidingPlace.transform.position, this.transform.position))
                        if (distanceFromPlayer < Vector3.Distance(hidingPlace.transform.position, Player.Value.transform.position))
                        {
                            Debug.Log("It is better");
                            myTargetHidingPlace = hidingPlace.transform;
                            //distanceToHidingPlace = Vector3.Distance(myTargetHidingPlace.position, this.transform.position);
                        }
                    }
                    else // Otherwise set my target place to the transform of the one I just found
                    {
                        Debug.Log("I don't have a hiding place, setting this one");
                        myTargetHidingPlace = hidingPlace.transform;
                        //distanceToHidingPlace = Vector3.Distance(myTargetHidingPlace.position, this.transform.position);
                    }
                }
            }

            //After checking all hiding places and getting the one that works best for me, set my target navigation to that
            agent.SetDestination(myTargetHidingPlace.Value.transform.position);
        }
    }
}
