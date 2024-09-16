using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ShootPlayer : Action
{
    public SharedTransform player; // Reference to the player's transform
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform shootingPoint; // Point from where the projectile will be shot
    public float projectileSpeed = 10f; // Speed of the projectile
    public bool shootParent = false;

    public override TaskStatus OnUpdate()
    {
        ShootProjectile();
        return TaskStatus.Success;
    }

    void ShootProjectile()
    {
        GameObject projectile = Object.Instantiate(projectilePrefab, shootingPoint.position, shootingPoint.rotation);
        Vector3 directionToPlayer = shootParent ? (player.Value.parent.position - shootingPoint.position).normalized : (player.Value.position - shootingPoint.position).normalized;
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = directionToPlayer * projectileSpeed;
    }
}
