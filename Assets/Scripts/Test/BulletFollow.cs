using UnityEngine;

public class BulletFollow : MonoBehaviour
{
    public string playerTag = "Player"; // Tag to identify the player
    public float speed = 5f; // Speed of the bullet

    private Transform player;

    void Start()
    {
        // Find the player by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        // Check if the player exists
        if (player != null)
        {
            // Calculate the direction from the bullet to the player
            Vector3 direction = player.position - transform.position;
            direction.Normalize();

            // Move the bullet towards the player
            transform.position += direction * speed * Time.deltaTime;
        }
    }
}
