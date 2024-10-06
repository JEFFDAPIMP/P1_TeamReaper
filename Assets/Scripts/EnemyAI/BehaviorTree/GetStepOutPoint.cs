using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GetStepOutPoint : Action
{
    public SharedTransform coverPosition; // Position of the cover
    private Transform peekSpot1;
    private Transform peekSpot2;
    public SharedTransform peekSpot;

    public override void OnStart()
    {
        peekSpot1 = coverPosition.Value.parent.transform.Find("PeekSpot");
        peekSpot2 = coverPosition.Value.parent.transform.Find("PeekSpot (1)");
    }

    public override TaskStatus OnUpdate()
    {
        if(Random.Range(0, 2) == 0)
        {
            peekSpot.Value = peekSpot1;
        }
        else
        {
            peekSpot.Value = peekSpot2;
        }
        return TaskStatus.Success;
    }
}
