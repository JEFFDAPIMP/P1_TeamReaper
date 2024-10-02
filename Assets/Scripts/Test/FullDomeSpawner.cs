using UnityEngine;

public class FullDomeSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // The object to spawn
    public float radius = 5f; // Radius of the dome
    public float spawnInterval = 1f; // Time interval between spawns

    void Start()
    {
        InvokeRepeating("SpawnObjects", 0f, spawnInterval);
    }

    void SpawnObjects()
    {
        for (int i = 0; i < 3; i++)
        {
            // Calculate spherical coordinates
            float theta = Random.Range(0, 2 * Mathf.PI); // Angle in the XY plane
            float phi = Random.Range(0, Mathf.PI); // Angle from the Z axis

            // Convert spherical coordinates to Cartesian coordinates
            float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
            float z = radius * Mathf.Cos(phi);

            // Create the position vector
            Vector3 position = new Vector3(x, y, z);

            // Instantiate the object at the calculated position
            Instantiate(objectToSpawn, position, Quaternion.identity);
        }
    }
}
