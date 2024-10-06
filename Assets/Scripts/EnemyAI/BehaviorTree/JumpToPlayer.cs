using UnityEngine;
//using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;

public class JumpToPlayer : Action
{
    public SharedGameObject player; // Reference to the player's transform
    public float jumpHeight = 5f; // Height of the jump
    public float jumpDuration = 1f; // Duration of the jump
    public float jumpDistance = 2f; // Distance from the player to land

    //private NavMeshAgent agent;
    private bool isJumping = false;

    public override void OnStart()
    {
        //agent = GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        if (!isJumping && Vector3.Distance(transform.position, player.Value.transform.position) > jumpDistance)
        {
            Debug.Log("running");
            StartCoroutine(JumpToPlayerCoroutine());
            return TaskStatus.Running;
        }
        return TaskStatus.Success;
    }

    IEnumerator JumpToPlayerCoroutine()
    {
        isJumping = true;
        //agent.enabled = false;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = player.Value.transform.position + (transform.position - player.Value.transform.position).normalized * jumpDistance;
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
        //agent.enabled = true;
        isJumping = false;
        yield return TaskStatus.Success;
    }
}
