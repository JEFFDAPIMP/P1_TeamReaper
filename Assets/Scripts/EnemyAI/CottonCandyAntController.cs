using System.Collections.Generic;
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

    [SerializeField] private List<CottonCandyAntController> cottonCandyAntsInSquad = new List<CottonCandyAntController>();
    private bool squadAlerted = false;

    private Transform playerTransform;
    [SerializeField] private GameObject chompPrefab;
    [SerializeField] private Transform chompPoint;
    [SerializeField] private float projectileSpeed;
    private float nextChompTime;
    [SerializeField] private float chompCooldown = 1.0f;

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
        //calculate health to see if we have taken damage
        float healthDifference = currentHealth - health.health;
        currentHealth = health.health;

        //If taken damage go after player
        if (healthDifference > 0)
        {
            isChasingPlayer = true;
        }

        // Check if player is within vision cone and visible via raycasting
        // If I can see player go after player
        if (IsPlayerInSight())
        {
            isChasingPlayer = true;
        }

        //If im chasing player, let squad know its time to bring the pain, else look for player.
        if (isChasingPlayer)
        {
            AlertSquad();
            Pursue();
        }
        else
        {
            Patrol();
        }
    }

    /// <summary>
    /// Method used to check if player is within vision cone
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Method used to tell enemyBase class we are partroling 
    /// </summary>
    private void Patrol()
    {
        enemyBase.patrolling = true;
        enemyBase.speed = walkSpeed;
    }

    /// <summary>
    /// Method used to tell enemyBase to go after player
    /// </summary>
    private void Pursue()
    {
        enemyBase.patrolling = false;
        enemyBase.speed = runSpeed;
        enemyBase.setPlayer(GameObject.FindGameObjectWithTag("Player"));

        if (enemyBase.isAtDestination())
        {
            //if we wait long enough then chomp
            if (Time.time >= nextChompTime)
            {
                Chomp();
                nextChompTime = Time.time + chompCooldown;
            }
            
        }
    }

    /// <summary>
    /// Method used to tell other ants that I found player and we should all go after the player.
    /// </summary>
    public void AlertSquad()
    {
        //If we have already alerted, then we are done.
        if (squadAlerted)
        {
            return;
        }

        squadAlerted = true;
        isChasingPlayer = true;

        foreach (CottonCandyAntController antController in cottonCandyAntsInSquad)
        {
            antController.AlertSquad();
        }
    }
    
    /// <summary>
    /// Instantiates a Chomp Bullet and sends it forward
    /// </summary>
    void Chomp()
    {
        //LookAtTarget();

        GameObject chomp = Instantiate(chompPrefab, chompPoint.position, Quaternion.identity);
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        chomp.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
    }

    /*
    private void LookAtTarget()
    {
        float rotationSpeed = 100.0f;

        // Calculate the direction to the target
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Calculate the rotation needed to look at the target 
        Quaternion lookRotation = Quaternion.LookRotation(direction); 
        
        // Rotate the object smoothly
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
    */

    /// <summary>
    /// Method used for debugging within the inspector
    /// If you want this off, just set debugMode to false.
    /// </summary>
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
