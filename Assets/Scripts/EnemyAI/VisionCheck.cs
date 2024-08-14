using UnityEngine;

/// <summary>
/// This class handles testing to see if this game object can raycast to player without interuptions
/// </summary>
public class VisionCheck : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player's transform

    private Vector3 offset = new Vector3(0, 1, 0); // transform offset to point at middle of player object VS players feet

    public bool CanSeePlayer()
    {
        //Vector3 directionToPlayer = player.position - transform.position;
        Vector3 directionToPlayer = (player.position + offset) - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                Debug.DrawLine(transform.position, (player.position + offset), Color.green);
                return true;
            }
        }

        Debug.DrawLine(transform.position, (player.position + offset), Color.red);
        return false;
    }
}
