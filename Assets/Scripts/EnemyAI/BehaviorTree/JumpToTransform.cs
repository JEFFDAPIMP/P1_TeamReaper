using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;

public class JumpToTransform : Action
{
    public SharedTransform targetTransform; // The target transform to jump to
    public float jumpHeight = 5f; // Height of the jump
    public float jumpDuration = 1f; // Duration of the jump

    private NavMeshAgent agent;
    private bool isJumping = false;

    public override void OnStart()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        if (!isJumping)
        {
            StartCoroutine(JumpToTarget());
            return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }

    IEnumerator JumpToTarget()
    {
        isJumping = true;
        agent.enabled = false;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = targetTransform.Value.position;
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float height = Mathf.Sin(Mathf.PI * t) * jumpHeight;
            transform.position = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * height;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        agent.enabled = true;
        isJumping = false;
        yield return TaskStatus.Success;
    }
}
