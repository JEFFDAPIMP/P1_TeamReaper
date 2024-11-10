using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public Transform centerPoint; // The point around which the object will circle
    public float speed = 5f; // Speed of the circular movement
    public float radius = 5f; // Radius of the circle

    private float angle = 0f;

    void Update()
    {
        angle += speed * Time.deltaTime; // Increment the angle based on speed
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        transform.position = new Vector3(centerPoint.position.x + x, centerPoint.position.y, centerPoint.position.z + z);
    }
}
