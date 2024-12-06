using UnityEngine;

public class TentacleController : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player
    [SerializeField] private Transform ikTarget; // 2D IK target
    [SerializeField] private Transform[] tentacleSegments; // Array of tentacle segments
    [SerializeField] private float maxScale = 2f; // Maximum scale factor
    [SerializeField] private float minScale = 0.5f; // Minimum scale factor
    [SerializeField] private float maxDistance = 10f; // Maximum distance for scaling

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        AdjustTentacleSegments();
    }

    void AdjustTentacleSegments()
    {
        float distanceFromTargetToPlayer = Vector3.Distance(ikTarget.position, player.position);

        for (int i = 0; i < tentacleSegments.Length; i++)
        {
            float distanceToIKTarget = Vector3.Distance(tentacleSegments[i].position, ikTarget.position);

            // Calculate the scale so that segments get smaller as the target gets closer to the player
            float scaleRatio = Mathf.InverseLerp(0, maxDistance, distanceToIKTarget);
            float scale = Mathf.Lerp(maxScale, minScale, scaleRatio);

            // Apply the calculated scale to the segment
            tentacleSegments[i].localScale = new Vector3(scale, scale, scale);
        }
    }
}
