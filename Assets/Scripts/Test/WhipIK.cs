using UnityEngine;
using System.Collections;

public class WhipIK : MonoBehaviour
{
    [SerializeField] private Transform[] bones; // Array of whip bones from base to tip
    [SerializeField] private Transform target; // Target for the end of the whip
    [SerializeField] private float whipSpeed = 5f; // Speed of the whip movement
    [SerializeField] private float whipMagnitude = 1f; // Magnitude of the whip snap

    private Vector3[] bonePositions; // Store positions of the bones
    private Vector3[] velocities; // Store velocities of the bones for smooth movement

    void Start()
    {
        // Initialize positions and velocities arrays
        bonePositions = new Vector3[bones.Length];
        velocities = new Vector3[bones.Length];
        for (int i = 0; i < bones.Length; i++)
        {
            bonePositions[i] = bones[i].position;
            velocities[i] = Vector3.zero;
        }
    }

    void Update()
    {
        WhipMovement();
    }

    private void WhipMovement()
    {
        // Start the movement from the base to mimic the motion of a whip
        bonePositions[0] = bones[0].position;

        // Apply the whip effect using a spring-damper approach
        for (int i = 1; i < bones.Length; i++)
        {
            Vector3 targetPosition = bonePositions[i - 1] + (target.position - bonePositions[i - 1]).normalized * whipMagnitude;
            bonePositions[i] = Vector3.SmoothDamp(bonePositions[i], targetPosition, ref velocities[i], 1 / whipSpeed);
        }

        // Apply positions to bones and ensure each bone looks at the next one
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].position = bonePositions[i];
            if (i < bones.Length - 1)
            {
                bones[i].LookAt(bones[i + 1].position);
            }
        }
    }
}
