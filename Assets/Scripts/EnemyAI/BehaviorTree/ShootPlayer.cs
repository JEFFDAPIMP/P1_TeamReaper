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
    public SharedFloat cooldownTime; // Time interval between shots
    public SharedFloat lastShotTime; // The time when the last shot was fired
    public SharedFloat currentTime; // Current time

    private Vector3 offset = new Vector3(0, 1, 0); // Transform offset to point at middle of player object vs player's feet

    public override TaskStatus OnUpdate()
    {
        //currentTime.Value = Time.time; // Update current time

        Debug.Log("Before update: " + currentTime.Value);
        currentTime.Value = Time.time;
        lastShotTime.Value = 12.1f;
        cooldownTime.Value = 12.1f;
        Debug.Log("After update: " + currentTime.Value);


        /*
        if (CanShoot())
        {
            ShootProjectile();
            lastShotTime.Value = currentTime.Value; // Update the last shot time
            return TaskStatus.Success;
        }
        */
        //return TaskStatus.Failure;
        return TaskStatus.Success;
    }

    public bool CanShoot()
    {
        return currentTime.Value - lastShotTime.Value >= cooldownTime.Value;
    }

    public void ShootProjectile()
    {
        GameObject projectile = Object.Instantiate(projectilePrefab, shootingPoint.position, shootingPoint.rotation);
        Vector3 directionToPlayer = shootParent ? (player.Value.parent.position - shootingPoint.position).normalized : (player.Value.position - shootingPoint.position).normalized;
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = directionToPlayer * projectileSpeed;
    }
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
