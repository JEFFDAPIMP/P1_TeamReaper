using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Controls Enemy base behavior
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBase : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private NavMeshAgent agent;

    public float speed = 3.5f;
    public float icedSpeed = 1f;

    public bool iced = false;
    public bool stuck = false;
    public bool wondering = false;

    //For enemy Web/stuck
    private Collider other;

    /// <summary>
    /// set NavMeshAgent agent variable when object is initialised, regardless of whether or not the script is enabled.
    /// </summary>
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        if (stuck && !other)
        {
            stuck = false;
        }

        if (wondering)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                Wander();
            }
        }
        else
        {
            agent.SetDestination(player.transform.position);
        }

        
        if(iced)
        {
            agent.speed = icedSpeed;
        }
        else if (stuck)
        {
            Debug.Log("enemy is stuck!");
            agent.speed = 0;
        }
        else
        {
            agent.speed = speed;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        this.other = other;
        if (other.transform.tag == "Sticky")
        {
            stuck = true;
        }
    }

    public void setPlayer(GameObject newPlayer)
    {
        this.player = newPlayer;
    }

    public void Wander()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 10f;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10f, 1);
        Vector3 finalPosition = hit.position;
        agent.SetDestination(finalPosition);
    }
}
