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

    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float icedSpeed = 1f;

    public bool iced = false;
    public bool stuck = false;

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

        agent.SetDestination(player.transform.position);
        if(iced)
        {
            agent.speed = icedSpeed;
        }
        else if (stuck)
        {
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
}
