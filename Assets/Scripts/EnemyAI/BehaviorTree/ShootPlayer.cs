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

    private Vector3 offset = new Vector3(0, 1, 0); // transform offset to point at middle of player object VS players feet

    public override TaskStatus OnUpdate()
    {
        //Debug.Log("Task updated: " + this.GetType().Name);
        //if(!canSeePlayer())
        //{
            //return TaskStatus.Failure;
        //}
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

    /*
    private bool canSeePlayer()
    {
        Vector3 directionToPlayer = (player.Value.position + offset) - transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                Debug.DrawLine(transform.position, (player.Value.position + offset), Color.green);
                return true;
            }
        }
        return false;
    }
    */
}
