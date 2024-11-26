using UnityEngine;

public class SlitheringTailIK : MonoBehaviour
{
    [SerializeField] private Transform[] tailBones; // Array of tail bones from base to tip
    [SerializeField] private float slitherSpeed = 2f; // Speed of the slithering movement
    [SerializeField] private float slitherMagnitude = 0.5f; // Magnitude of the slithering movement
    [SerializeField] private float slitherFrequency = 1f; // Frequency of the slithering movement

    private Vector3[] originalPositions; // Store original positions of the bones

    void Start()
    {
        // Initialize original positions
        originalPositions = new Vector3[tailBones.Length];
        for (int i = 0; i < tailBones.Length; i++)
        {
            originalPositions[i] = tailBones[i].localPosition;
        }
    }

    void Update()
    {
        Slither();
    }

    private void Slither()
    {
        float time = Time.time * slitherSpeed;

        for (int i = 0; i < tailBones.Length; i++)
        {
            // Calculate the offset for the slithering movement using a sine wave
            float offset = Mathf.Sin(time + i * slitherFrequency) * slitherMagnitude;

            // Apply the offset to the original position to create the slithering effect
            Vector3 slitherPosition = originalPositions[i];
            slitherPosition.x += offset;

            tailBones[i].localPosition = slitherPosition;

            // Make sure each bone looks at the next one to maintain the tail's structure
            if (i < tailBones.Length - 1)
            {
                tailBones[i].LookAt(tailBones[i + 1].position);
            }
        }
    }
}
