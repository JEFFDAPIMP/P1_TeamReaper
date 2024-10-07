using UnityEngine;

[RequireComponent(typeof(EnemyBase))]
[RequireComponent(typeof(Health))]
public class SnowconeManController : MonoBehaviour
{
    public GameObject snowballPrefab;
    public GameObject punchPrefab;
    public Transform player;
    public float throwCooldown = 2f;
    public float rageSpeed = 5f;
    public float normalSpeed = 0f;
    public float punchAttackRange = 1f;
    public float snowballAttackRange = 1f;
    public float projectileSpeed = 10f;
    private bool isRaging = false;
    private float nextThrowTime = 0f;
    private EnemyBase enemyBase;
    private Health health;
    private Animator animator;
    [SerializeField] private Transform shootPoint;
    private float rotationSpeed = 180;

    void Start()
    {
        enemyBase = GetComponent<EnemyBase>();
        health = GetComponent<Health>();
        animator = GetComponentInChildren<Animator>();
        enemyBase.speed = normalSpeed;
    }

    void Update()
    {
        if(health.health < health.maxHealth)
        {
            isRaging = true;
        }
        if (isRaging)
        {
            RageBehavior();
        }
        else
        {
            NormalBehavior();
        }
    }

    void NormalBehavior()
    {
        enemyBase.speed = normalSpeed;
        RotateTowardsPlayer();
        //if out of range do nothing.
        if (Vector3.Distance(transform.position, player.position) > snowballAttackRange)
        {
            return;
        }
        //if we wait long enough throw snowball
        if (Time.time >= nextThrowTime)
        {
            ThrowSnowball();
            nextThrowTime = Time.time + throwCooldown;
        }
    }

    void RageBehavior()
    {
        animator.SetBool("Raging", true);
        enemyBase.speed = rageSpeed;

        if (Vector3.Distance(transform.position, player.position) > punchAttackRange)
        {
            return;
        }

        if (Time.time >= nextThrowTime)
        {
            ThrowPunch();
            nextThrowTime = Time.time + throwCooldown;
        }
    }

    void ThrowSnowball()
    {
        animator.SetTrigger("Throwing");
        GameObject snowball = Instantiate(snowballPrefab, shootPoint.position, Quaternion.identity);
        Vector3 direction = (player.position - transform.position).normalized;
        snowball.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
    }

    void ThrowPunch()
    {
        GameObject snowball = Instantiate(punchPrefab, shootPoint.position, Quaternion.identity);
        Vector3 direction = (player.position - transform.position).normalized;
        snowball.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
