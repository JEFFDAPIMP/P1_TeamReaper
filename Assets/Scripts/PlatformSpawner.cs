using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platformPrefab; // Reference to the platform prefab
    public float spawnOffset = 0.5f;  // Distance to offset the platform from the collision point

    void OnTriggerEnter(Collider other)
    {
        // Get the collision point and the surface normal
        Vector3 collisionPoint = other.ClosestPoint(transform.position);
        Vector3 surfaceNormal = (transform.position - collisionPoint).normalized;

        // Calculate the spawn position by offsetting along the surface normal
        Vector3 spawnPosition = collisionPoint + surfaceNormal * spawnOffset;

        // Instantiate the platform at the adjusted position
        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        // Optionally, destroy the bullet upon collision
        Destroy(gameObject);
    }

    
    void OnCollisionEnter(Collision collision)
    {
        // Get the collision point and the surface normal
        ContactPoint contactPoint = collision.contacts[0];
        Vector3 collisionPoint = contactPoint.point;
        Vector3 surfaceNormal = contactPoint.normal;

        // Calculate the spawn position by offsetting along the surface normal
        Vector3 spawnPosition = collisionPoint + surfaceNormal * spawnOffset;

        // Instantiate the platform at the adjusted position
        Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

        // Optionally, destroy the bullet upon collision
        Destroy(gameObject);
    }
    
}
