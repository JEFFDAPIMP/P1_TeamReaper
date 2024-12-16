using UnityEngine;
using System.Collections; // Added for IEnumerator
using static Health;

/// <summary>
/// Handles all actions of the assigned forgotten chocolate arm IK target
/// </summary>
public class ForgottenChocolateArmController : MonoBehaviour
{
    [SerializeField] private GameObject armIKTarget;
    [SerializeField] private GameObject player; // Reference to the player GameObject
    [SerializeField] private float slamHeight = 10f; // Height to reach before slamming down
    [SerializeField] private float slamSpeed = 5f; // Speed of the slam
    [SerializeField] private float swipeSpeed = 5f; // Speed of the swipe
    [SerializeField] private float swingAngle = 45f; // Angle of the swing in degrees
    [SerializeField] private float manualSwingDistance = 5f; // Manually set distance of the swing
    [SerializeField] private float windUpDistance = 3f; // Distance to move to the side for wind-up
    [SerializeField] private Damage[] bones; // Reference to all arm bones that contain Damage Script


    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = armIKTarget.transform.position;

        // Ensure the player reference is set
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    /// <summary>
    /// Move arm IK target up into the air then fall on the player's current location from when this was called.
    /// </summary>
    public void slamAttack()
    {
        SetDamageTypeOfAllBones(Health.allDamageType.Crunch);
        StartCoroutine(SlamRoutine());
    }

    private IEnumerator SlamRoutine()
    {
        Vector3 targetPosition = new Vector3(armIKTarget.transform.position.x, slamHeight, armIKTarget.transform.position.z);

        // Move up
        while (Vector3.Distance(armIKTarget.transform.position, targetPosition) > 0.1f)
        {
            armIKTarget.transform.position = Vector3.MoveTowards(armIKTarget.transform.position, targetPosition, slamSpeed * Time.deltaTime);
            yield return null;
        }

        // Move down to player's position
        targetPosition = new Vector3(player.transform.position.x, originalPosition.y, player.transform.position.z);
        while (Vector3.Distance(armIKTarget.transform.position, targetPosition) > 0.1f)
        {
            armIKTarget.transform.position = Vector3.MoveTowards(armIKTarget.transform.position, targetPosition, slamSpeed * Time.deltaTime);
            yield return null;
        }

        // Return to original position
        armIKTarget.transform.position = originalPosition;
    }

    /// <summary>
    /// Move arm IK target up, then get into position before moving in an arch around at the player based on the current player height from when this was called.
    /// </summary>
    public void swipeAttack()
    {
        SetDamageTypeOfAllBones(Health.allDamageType.Wet);
        StartCoroutine(SwipeRoutine());
    }

    private IEnumerator SwipeRoutine()
    {
        Vector3 startPosition = armIKTarget.transform.position;
        Vector3 upPosition = new Vector3(startPosition.x, slamHeight, startPosition.z);

        // Get player's current position
        Vector3 playerPosition = player.transform.position;

        // Move up
        while (Vector3.Distance(armIKTarget.transform.position, upPosition) > 0.1f)
        {
            armIKTarget.transform.position = Vector3.MoveTowards(armIKTarget.transform.position, upPosition, slamSpeed * Time.deltaTime);
            yield return null;
        }

        // Wind up to the side
        Vector3 windUpPosition = upPosition + transform.right * windUpDistance;
        while (Vector3.Distance(armIKTarget.transform.position, windUpPosition) > 0.1f)
        {
            armIKTarget.transform.position = Vector3.MoveTowards(armIKTarget.transform.position, windUpPosition, swipeSpeed * Time.deltaTime);
            yield return null;
        }

        // Calculate initial swipe position
        Vector3 initialSwipePosition = new Vector3(startPosition.x, playerPosition.y, startPosition.z);

        // Calculate the total swing distance
        float halfSwingDistance = manualSwingDistance / 2.0f;

        // Ensure that the middle of the arm (initialSwipePosition) passes through the player's position
        Vector3 directionToPlayer = (playerPosition - initialSwipePosition).normalized;
        float halfSwingAngle = swingAngle / 2.0f;

        // Calculate the start and end positions of the swing
        Vector3 startSwingPosition = Quaternion.Euler(0, -halfSwingAngle, 0) * directionToPlayer * halfSwingDistance + playerPosition;
        Vector3 endSwingPosition = Quaternion.Euler(0, halfSwingAngle, 0) * directionToPlayer * halfSwingDistance + playerPosition;

        // Perform the swipe in an arc towards and past the player
        float swipeDuration = Vector3.Distance(startSwingPosition, endSwingPosition) / swipeSpeed;
        float elapsedTime = 0;
        while (elapsedTime < swipeDuration)
        {
            float t = elapsedTime / swipeDuration;
            Vector3 currentSwipePosition = Vector3.Lerp(startSwingPosition, endSwingPosition, t);
            armIKTarget.transform.position = currentSwipePosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return to original position
        armIKTarget.transform.position = originalPosition;
    }

    /// <summary>
    /// Set damage type of all bones in the arm
    /// </summary>
    /// <param name="damageType"> Health enum Damage Type</param>
    private void SetDamageTypeOfAllBones(Health.allDamageType damageType)
    {
        foreach(Damage bone in bones)
        {
            bone.SetDamageType(damageType);
        }
    }
}
