using UnityEngine;

public class BananaBoomerang : MonoBehaviour
{
    private Transform player;
    private Transform boss;
    private Vector3 centerPoint;
    private float speed;
    private float rotationSpeed;
    private Vector3 startPosition;
    private float angle;
    private float height;

    [SerializeField] private bool followMode;

    public void Initialize(Transform player, Transform boss, float speed, float rotationSpeed)
    {
        this.player = player;
        this.boss = boss;
        this.centerPoint = (boss.position + player.position) / 2; // Calculate center point
        this.speed = speed;
        this.rotationSpeed = rotationSpeed;
        startPosition = transform.position;
        angle = Mathf.Atan2(startPosition.z - centerPoint.z, startPosition.x - centerPoint.x); // Set the initial angle based on position

        this.height = player.position.y+1;
    }

    void Update()
    {
        angle -= speed * Time.deltaTime;
        float x, z;

        if (followMode)
        {
            x = Mathf.Cos(angle) * Vector3.Distance(centerPoint, player.position); //This will make it home in on the player as it moves
            z = Mathf.Sin(angle) * Vector3.Distance(centerPoint, player.position); //This will make it home in on the player as it moves
        }
        else
        {
            x = Mathf.Cos(angle) * Vector3.Distance(centerPoint, startPosition);
            z = Mathf.Sin(angle) * Vector3.Distance(centerPoint, startPosition);
        }

        transform.position = new Vector3(centerPoint.x + x, height, centerPoint.z + z);

        transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
        //transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}