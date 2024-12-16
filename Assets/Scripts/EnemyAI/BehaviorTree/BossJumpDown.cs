using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class BossJumpDown : Action
{
    public float fallDuration = 1f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float elapsedTime;

    public override void OnStart()
    {
        startPosition = transform.position;
        endPosition = new Vector3(startPosition.x, startPosition.y - startPosition.y, startPosition.z); // Assuming falling to the ground level
        elapsedTime = 0f;
    }

    public override TaskStatus OnUpdate()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / fallDuration;
        transform.position = Vector3.Lerp(startPosition, endPosition, t);

        if (t >= 1f)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}
