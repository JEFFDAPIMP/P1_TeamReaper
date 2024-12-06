using UnityEngine;
using System.Collections;

public class TailIK2 : MonoBehaviour
{
    [SerializeField] private Transform[] tailBones; // Array of tail bones from base to tip
    [SerializeField] private Transform target; // Target for the end of the tail
    [SerializeField] private bool controlTail = true; // Toggle for using the target or not
    [SerializeField] private float movementSpeed = 5f; // Speed of the tail movement
    [SerializeField] private float movementMagnitude = 1f; // Magnitude of the tail movement
    [SerializeField] private Transform root; // The root object that moves the tail

    private Vector3[] bonePositions; // Store positions of the bones
    private Vector3[] velocities; // Store velocities of the bones for smooth movement
    private Vector3 previousRootPosition; // Track previous position of the root object

    void Start()
    {
        // Initialize positions and velocities arrays
        bonePositions = new Vector3[tailBones.Length];
        velocities = new Vector3[tailBones.Length];
        for (int i = 0; i < tailBones.Length; i++)
        {
            bonePositions[i] = tailBones[i].position;
            velocities[i] = Vector3.zero;
        }

        previousRootPosition = root.position; // Set initial root position
    }

    void Update()
    {
        if (controlTail)
        {
            MoveTowardsTarget();
        }
        else
        {
            //NaturalMovement();
            //MoveWithRoot();
            MoveWithRootSmoothly();
        }
    }

    private void MoveTowardsTarget()
    {
        // Start the movement from the base
        bonePositions[0] = tailBones[0].position;

        // Apply the movement effect towards the target
        for (int i = 1; i < tailBones.Length; i++)
        {
            Vector3 targetPosition = bonePositions[i - 1] + (target.position - bonePositions[i - 1]).normalized * movementMagnitude;
            bonePositions[i] = Vector3.SmoothDamp(bonePositions[i], targetPosition, ref velocities[i], 1 / movementSpeed);
        }

        // Apply positions to bones and ensure each bone looks at the next one
        for (int i = 0; i < tailBones.Length; i++)
        {
            tailBones[i].position = bonePositions[i];
            if (i < tailBones.Length - 1)
            {
                tailBones[i].LookAt(tailBones[i + 1].position);
            }
        }
    }

    private void NaturalMovement()
    {
        float time = Time.time * movementSpeed;

        for (int i = 0; i < tailBones.Length; i++)
        {
            // Calculate the natural movement using a sine wave
            float offset = Mathf.Sin(time + i * movementMagnitude) * movementMagnitude;

            // Apply the offset to the original position to create the natural tail movement effect
            Vector3 naturalPosition = bonePositions[i];
            naturalPosition.x += offset;

            tailBones[i].position = naturalPosition;

            // Make sure each bone looks at the next one to maintain the tail's structure
            if (i < tailBones.Length - 1)
            {
                tailBones[i].LookAt(tailBones[i + 1].position);
            }
        }
    }

    private void MoveWithRoot()
    {
        // Calculate the movement of the root object
        Vector3 rootMovement = root.position - previousRootPosition;

        // Apply the root movement to the tail bones using SmoothDamp
        for (int i = 0; i < tailBones.Length; i++)
        {
            bonePositions[i] = Vector3.SmoothDamp(bonePositions[i], bonePositions[i] + rootMovement * movementMagnitude, ref velocities[i], 1 / movementSpeed);
            tailBones[i].position = bonePositions[i];

            if (i < tailBones.Length - 1)
            {
                tailBones[i].LookAt(tailBones[i + 1].position);
            }
        }

        // Update the previous root position
        previousRootPosition = root.position;
    }

    private void MoveWithRootSmoothly()
    {
        // Start the movement from the base
        bonePositions[0] = tailBones[0].position;

        // Apply the movement based on the root object's movement
        for (int i = 1; i < tailBones.Length; i++)
        {
            Vector3 targetPosition = bonePositions[i - 1] + (root.position - previousRootPosition) * movementMagnitude;
            bonePositions[i] = Vector3.SmoothDamp(bonePositions[i], targetPosition, ref velocities[i], 1 / movementSpeed);
        }

        // Apply positions to bones and ensure each bone looks at the next one
        for (int i = 0; i < tailBones.Length; i++)
        {
            tailBones[i].position = bonePositions[i];
            if (i < tailBones.Length - 1)
            {
                tailBones[i].LookAt(tailBones[i + 1].position);
            }
        }

        // Update the previous root position
        previousRootPosition = root.position;
    }
}

