using UnityEngine;

/// <summary>
/// Instantiates set game object at location where this object trigger is entered then delets this object
/// </summary>
[RequireComponent(typeof(Collider))]
public class instantiateObjectOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private GameObject ObjectToInstantiate;

    /// <summary>
    /// Turn the collider into a trigger when object is initialised, regardless of whether or not the script is enabled.
    /// </summary>
    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    /// <summary>
    /// When trigger is entered, get the transform of it and instantiate set gameobject at that location then delete this object
    /// </summary>
    /// <param name="other"> other game object we collided with</param>
    private void OnTriggerEnter(Collider other)
    {
        Instantiate(ObjectToInstantiate, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}
