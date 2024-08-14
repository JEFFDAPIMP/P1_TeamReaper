using System.Collections;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Placed on Target Object to move to new transform
/// </summary>
public class MoveTargetToNewWaypoint : MonoBehaviour
{
    public bool useTimer = true;
    public int secondsBeforeChange = 3;
    public GameObject[] locationsToScycleThough;
    private int currentPositon = 0;
    private bool changing = false;

    private void Awake()
    {
        if(locationsToScycleThough.Length == 0)
        {
            Debug.LogError("To use MoveTargetToNewWaypoint, you must have minimum 1 position set");
        }
        setPositionRotation();
    }

    // Update is called once per frame
    void Update()
    {
        if (useTimer)
        {
            if (!changing) { 
                StartCoroutine(waitForTimeThenChangePosition(secondsBeforeChange));
            }
        }
    }

    private void setPositionRotation()
    {
        this.transform.position = locationsToScycleThough[currentPositon].transform.position;
        this.transform.rotation = locationsToScycleThough[currentPositon].transform.rotation;
    }

    private void advanceIndexMarker()
    {
        if(currentPositon < locationsToScycleThough.Length-1)
        {
            currentPositon++;
        }
        else
        {
            currentPositon = 0;
        }
    }

    IEnumerator waitForTimeThenChangePosition(int secondsToWait)
    {
        changing = true;
        yield return new WaitForSeconds(secondsToWait);
        advanceIndexMarker();
        setPositionRotation();
        changing = false;
    }
}
