using UnityEngine;
using System.Collections;

public class SetPositionArray : MonoBehaviour
{
    [SerializeField] private Transform[] positions; // Array of transforms to move between
    [SerializeField] private GameObject target; // Target GameObject to move
    [SerializeField] private float interval = 2.0f; // Time interval between position changes

    private int currentIndex = 0; // Current index in the array

    void Start()
    {
        if (positions.Length > 0 && target != null)
        {
            StartCoroutine(MoveTarget());
        }
    }

    private IEnumerator MoveTarget()
    {
        while (true)
        {
            // Set target position to the current position in the array
            target.transform.position = positions[currentIndex].position;

            // Increment index and loop back to the start if necessary
            currentIndex = (currentIndex + 1) % positions.Length;

            // Wait for the specified interval before moving to the next position
            yield return new WaitForSeconds(interval);
        }
    }
}
