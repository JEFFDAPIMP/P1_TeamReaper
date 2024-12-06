using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class BossJumpUp : Action
{
    public float jumpHeight = 5f;
    public float jumpDuration = 1f;
    private Vector3 startPosition;
    private Vector3 peakPosition;
    private float elapsedTime;

    public override void OnStart()
    {
        startPosition = transform.position;
        peakPosition = startPosition + Vector3.up * jumpHeight;
        elapsedTime = 0f;
    }

    public override TaskStatus OnUpdate()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / jumpDuration;
        float height = Mathf.Sin(Mathf.PI * t) * jumpHeight;
        transform.position = Vector3.Lerp(startPosition, peakPosition, t) + Vector3.up * height;

        if (t >= 1f)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
