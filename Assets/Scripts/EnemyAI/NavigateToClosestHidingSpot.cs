using UnityEngine;
using UnityEngine.AI;

public class NavigateToClosestHidingSpot : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private Transform Player;
    [SerializeField] private VisionCheck[] HidingPlaces;
    public Transform myTargetHidingPlace;
    //public float distanceToHidingPlace = Mathf.Infinity;
    public float distanceFromPlayer = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!myTargetHidingPlace)
        {
            setHidingPlace();
        }
        if (myTargetHidingPlace.GetComponent<VisionCheck>().CanSeePlayer())
        {
            //distanceToHidingPlace = Mathf.Infinity;
            setHidingPlace();
        }
        
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
                if (myTargetHidingPlace)
                {
                    Debug.Log("I have a hiding place, Checking to see if its better");
                    //if (distanceToHidingPlace > Vector3.Distance(hidingPlace.transform.position, this.transform.position))
                    if (distanceFromPlayer < Vector3.Distance(hidingPlace.transform.position, Player.position))
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
        agent.SetDestination(myTargetHidingPlace.transform.position);
    }
}
