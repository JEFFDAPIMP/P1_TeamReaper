using UnityEngine;

/// <summary>
/// When this object hits something that it can grapple to, pull the player to it.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(DeleteAfterTime))]
public class GrappleHook : MonoBehaviour
{

    [SerializeField] private GameObject player;
    private string playerTag = "Player";

    [SerializeField] private LayerMask grappleableLayer;
    private bool isGrappling = false;
    [SerializeField] private float hookSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Highlander rules, there can only be one, otherwise it turns into a tug of war
        if (GameObject.FindGameObjectsWithTag("GrappleHook").Length > 1)
        {
            Destroy(this.gameObject);
        }

        if (player == null) player = GameObject.FindGameObjectWithTag(playerTag);
    }

    private void Update()
    {
        if (isGrappling)
        {
            this.GetComponent<DeleteAfterTime>().enabled = false;

            Vector3 direction = (this.transform.position - player.transform.position).normalized;
            player.transform.position = Vector3.MoveTowards(player.transform.position, this.transform.position, hookSpeed * Time.deltaTime);

            // Stop grappling if the player reaches the grapple point
            if (Vector3.Distance(player.transform.position, this.transform.position) < 0.5f) 
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        //I want the option to grapple to specific objects instead of everything I see.
        if (IsInLayerMask(other.gameObject, grappleableLayer))
        {
            isGrappling=true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Method to check if the object's layer is within the specified LayerMask
    /// Method shifts the LayerMask value to the left by the object's layer number and performs a bitwise AND operation. If the result is non-zero, the object is within the LayerMask.
    /// </summary>
    /// <param name="obj">The GameObject to check</param>
    /// <param name="mask">The LayerMask to check against</param>
    /// <returns>True if the object's layer is within the LayerMask, otherwise False</returns>
    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }

}
