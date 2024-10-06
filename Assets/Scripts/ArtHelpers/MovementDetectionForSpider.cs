using UnityEngine;

public class MovementDetectionForSpider : MonoBehaviour
{
    public Transform player;
    private Vector3 lastPosition;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 movementDirection = currentPosition - lastPosition;
        Vector3 playerDirection = player.position - currentPosition;

        // Check if moving towards or away from the player
        float dotProduct = Vector3.Dot(movementDirection.normalized, playerDirection.normalized);
        if (dotProduct > 0)
        {
            animator.SetBool("Forward", true);
            animator.SetBool("Backward", false);
            Debug.Log("Moving towards the player");
        }
        else if (dotProduct < 0)
        {
            animator.SetBool("Forward", false);
            animator.SetBool("Backward", true);
            Debug.Log("Moving away from the player");
        }

        // Check if moving left or right relative to the player
        Vector3 crossProduct = Vector3.Cross(playerDirection, movementDirection);
        if (crossProduct.y > 0)
        {
            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
            Debug.Log("Moving to the right of the player");
        }
        else if (crossProduct.y < 0)
        {
            animator.SetBool("Right", false);
            animator.SetBool("Left", true);
            Debug.Log("Moving to the left of the player");
        }

        lastPosition = currentPosition;
    }
}
