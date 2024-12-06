using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class ProbabilitySelector : Composite
{
    public float probabilityOfFirst = 75f; // Probability of selecting the first child (in percent)
    private int currentChildIndex = 0;

    public override void OnStart()
    {
        // Determine which child to execute based on the probability
        float rand = Random.Range(0f, 100f);
        currentChildIndex = (rand < probabilityOfFirst) ? 0 : 1;
    }

    public override int CurrentChildIndex()
    {
        return currentChildIndex;
    }

    public override bool CanExecute()
    {
        // Continue execution until the selected child finishes
        return currentChildIndex < children.Count && (currentChildIndex == 0 || currentChildIndex == 1);
    }

    public override void OnChildExecuted(TaskStatus childStatus)
    {
        // Mark the composite as complete once the selected child finishes
        currentChildIndex = children.Count;
    }

    public override void OnEnd()
    {
        // Reset the composite to start
        currentChildIndex = 0;
    }
}
