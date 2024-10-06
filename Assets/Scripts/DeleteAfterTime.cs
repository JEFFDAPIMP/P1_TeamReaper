using System.Collections;
using UnityEngine;

/// <summary>
/// Delete this game object after given time
/// </summary>
public class DeleteAfterTime : MonoBehaviour
{
    [SerializeField]private float numberOfSecondsBeforeDelete = 3.0f;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        StartCoroutine(deleteMe(numberOfSecondsBeforeDelete));
    }

    /// <summary>
    /// Wait a number of seconds then delete this gameobject
    /// </summary>
    /// <param name="secondsToWait">float number of seconds to wait</param>
    /// <returns>null - deletes this game object after time</returns>
    IEnumerator deleteMe(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        Destroy(this.gameObject);
    }
}
