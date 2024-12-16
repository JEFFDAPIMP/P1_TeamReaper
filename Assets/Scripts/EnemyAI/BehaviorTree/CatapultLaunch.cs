using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CatapultLaunch : Action
{
    public SharedGameObject projectilePrefab;
    public SharedTransform player;
    public SharedTransform launchPoint;

    public override TaskStatus OnUpdate()
    {
        LaunchProjectile();
        return TaskStatus.Success;
    }

    void LaunchProjectile()
    {
        if (projectilePrefab.Value != null && player.Value != null && launchPoint.Value != null)
        {
            GameObject projectile = Object.Instantiate(projectilePrefab.Value, launchPoint.Value.position, launchPoint.Value.rotation);
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            // Calculate the launch velocity needed to reach the player
            Vector3 launchVelocity = CalculateLaunchVelocity(launchPoint.Value.position, player.Value.position);

            // Apply calculated velocity to the projectile
            projectileRb.velocity = launchVelocity;
        }
    }

    Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target)
    {
        // Calculate the distance and direction to the target
        Vector3 direction = target - start;
        float horizontalDistance = new Vector3(direction.x, 0, direction.z).magnitude;
        float verticalDistance = direction.y;
        float gravity = Physics.gravity.y;

        // Launch angle in radians (45 degrees)
        float launchAngle = 45f * Mathf.Deg2Rad;

        // Ensure horizontalDistance is positive and non-zero
        if (horizontalDistance <= 0)
        {
            return Vector3.zero;
        }

        // Calculate the initial velocity needed to reach the target
        float initialVelocity = Mathf.Sqrt((horizontalDistance * -gravity) / (Mathf.Sin(2 * launchAngle)));

        // Calculate the launch velocity components
        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;
        Vector3 launchVelocity = horizontalDirection * initialVelocity * Mathf.Cos(launchAngle);
        launchVelocity.y = initialVelocity * Mathf.Sin(launchAngle);

        return launchVelocity;
    }
}
