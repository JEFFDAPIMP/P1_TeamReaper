using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Shoot : Action
{
    public SharedGameObject player;
    public GameObject projectilePrefab;
    public Transform shootingPoint;
    public float projectileSpeed = 10f;
    public float shootingRange = 10f;

    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(transform.position, player.Value.transform.position) > shootingRange)
        {
            return TaskStatus.Failure;
        }

        ShootProjectile();
        return TaskStatus.Success;
    }

    void ShootProjectile()
    {
        GameObject projectile = Object.Instantiate(projectilePrefab, shootingPoint.position, shootingPoint.rotation);
        Vector3 directionToPlayer = (player.Value.transform.position - shootingPoint.position).normalized;
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = directionToPlayer * projectileSpeed;
    }
}
