using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ThrowBananaBoomerang : Action
{
    public GameObject bananaBoomerangPrefab;
    public SharedTransform player;
    //public Transform boss; // Boss transform
    public Transform throwPoint;
    public float speed = 5f;
    public float rotationSpeed = 360f;

    public override TaskStatus OnUpdate()
    {
        GameObject boomerang = Object.Instantiate(bananaBoomerangPrefab, throwPoint.position, throwPoint.rotation);
        BananaBoomerang boomerangScript = boomerang.GetComponent<BananaBoomerang>();
        boomerangScript.Initialize(player.Value, this.transform, speed, rotationSpeed);
        return TaskStatus.Success;
    }
}