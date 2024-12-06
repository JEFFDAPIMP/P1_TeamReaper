using UnityEngine;
using System.Collections.Generic;

public class TailIK : MonoBehaviour
{
    [SerializeField] private Transform target; // The target the tail should follow
    [SerializeField] private Transform[] bones; // The array of tail bones
    [SerializeField] private int iterations = 10; // Number of FABRIK iterations
    [SerializeField] private float tolerance = 0.01f; // Tolerance for the end bone position

    private List<Vector3> positions; // Positions of the bones

    void Start()
    {
        positions = new List<Vector3>(bones.Length);
        foreach (var bone in bones)
        {
            positions.Add(bone.position);
        }
    }

    void Update()
    {
        FABRIK();
    }

    private void FABRIK()
    {
        // Step 1: Forward reaching
        positions[positions.Count - 1] = target.position;
        for (int i = positions.Count - 2; i >= 0; i--)
        {
            float length = (bones[i + 1].position - bones[i].position).magnitude;
            positions[i] = positions[i + 1] + (positions[i] - positions[i + 1]).normalized * length;
        }

        // Step 2: Backward reaching
        positions[0] = bones[0].position;
        for (int i = 1; i < positions.Count; i++)
        {
            float length = (bones[i].position - bones[i - 1].position).magnitude;
            positions[i] = positions[i - 1] + (positions[i] - positions[i - 1]).normalized * length;
        }

        // Apply the calculated positions to the bones
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].position = positions[i];
            if (i < bones.Length - 1)
            {
                bones[i].LookAt(bones[i + 1]);
            }
        }
    }
}
