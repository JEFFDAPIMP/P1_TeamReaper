using UnityEngine;


//TODO need to find out why this isn't working
public class FallToGroundThenChangeToTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground") 
        {
            Debug.Log("FOUND THE GROUND 1 ");
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Collider>().isTrigger = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            Debug.Log("FOUND THE GROUND 2");
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Collider>().isTrigger = true;
        }
    }
}
