using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class CircularShooting : Action
{
    public GameObject bulletPrefab;
    public int numberOfBullets = 10;
    public float radius = 5f;
    public float bulletSpeed = 10f;
    public float yOffset = 1f;

    public override TaskStatus OnUpdate()
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfBullets;
            Vector3 bulletDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized;
            Vector3 spawnPosition = transform.position + bulletDirection * radius + Vector3.up * yOffset;

            GameObject bullet = Object.Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.velocity = bulletDirection * bulletSpeed;
        }

        return TaskStatus.Success;
    }
}
