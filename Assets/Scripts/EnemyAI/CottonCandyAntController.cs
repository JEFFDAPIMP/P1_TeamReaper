using UnityEngine;

[RequireComponent(typeof(EnemyBase))]
[RequireComponent(typeof(Health))]
public class CottonCandyAntController : MonoBehaviour
{
    private EnemyBase enemyBase;
    private Health health;
    private Animator animator;
    private float normalSpeed;
    private float currentHealth;
    private bool isChasingPlayer = false;

    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float sightRange = 10f; // Sight range for detecting player
    [SerializeField] private float fovAngle = 45f; // Field of view angle for the vision cone
    [SerializeField] private bool debugMode = false; // Toggle for debug mode

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        enemyBase = GetComponent<EnemyBase>();
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        normalSpeed = enemyBase.speed;
        currentHealth = health.health;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float healthDifference = currentHealth - health.health;
        currentHealth = health.health;

        if (healthDifference > 0)
        {
            isChasingPlayer = true;
        }

        // Check if player is within vision cone and visible via raycasting
        if (IsPlayerInSight())
        {
            isChasingPlayer = true;
            //animator.SetBool("IsChasing", true);
        }
        else
        {
            isChasingPlayer = false;
            //animator.SetBool("IsChasing", false);
        }

        if (isChasingPlayer)
        {
            Pursue();
        }
        else
        {
            Patrol();
        }
    }

    private bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < fovAngle / 2 && directionToPlayer.magnitude <= sightRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, sightRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void Patrol()
    {
        enemyBase.patrolling = true;
        enemyBase.speed = walkSpeed;
    }

    private void Pursue()
    {
        enemyBase.patrolling = false;
        enemyBase.speed = runSpeed;
        enemyBase.setPlayer(GameObject.FindGameObjectWithTag("Player"));
    }

    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sightRange);

            Vector3 fovLine1 = Quaternion.AngleAxis(fovAngle / 2, transform.up) * transform.forward * sightRange;
            Vector3 fovLine2 = Quaternion.AngleAxis(-fovAngle / 2, transform.up) * transform.forward * sightRange;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, fovLine1);
            Gizmos.DrawRay(transform.position, fovLine2);

            if (playerTransform != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(transform.position, (playerTransform.position - transform.position).normalized * sightRange);
            }
        }
    }
}
