using UnityEngine;

/// <summary>
/// Handles all actions of the forgotten chocolate monster
/// </summary>
public class ForgottenChocolateController : MonoBehaviour
{
    [SerializeField] private ForgottenChocolateArmController[] armList; // Array of arms
    [SerializeField] private Transform player; // Reference to the player
    [SerializeField] private float attackInterval = 3f; // Time interval between attacks
    [Range(0f, 1f)][SerializeField] private float slamAttackProbability = 0.5f; // Probability threshold for attack type (0 to 1)
    [SerializeField] private Animator animator;
    private float attackTimer;

    void Start()
    {
        attackTimer = attackInterval;
        if (animator == null)
        {
            Debug.LogError("ForgottenChocolateController is missing Animator reference");
        }
    }

    /// <summary>
    /// Wait some time, then attack with either left or right arm
    /// </summary>
    private void Update()
    {
        Attack();
    }

    /// <summary>
    /// Choose an arm point from an array that is closest to the player.
    /// Perform a slam or swipe attack with the selected arm.
    /// </summary>
    private void Attack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            attackTimer = attackInterval;

            // Choose the arm closest to the player
            ForgottenChocolateArmController chosenArm = GetClosestArmToPlayer();
            if (chosenArm != null)
            {
                // Use the attackTypeProbability to decide the attack type
                if (Random.value < slamAttackProbability)
                {
                    animator.SetBool("AttackLeft", true);
                    chosenArm.slamAttack();
                }
                else
                {
                    animator.SetBool("AttackRight", true);
                    chosenArm.swipeAttack();
                }
            }
        }
        else
        {
            animator.SetBool("AttackLeft", false);
            animator.SetBool("AttackRight", false);
        }
    }

    /// <summary>
    /// Determines which arm in the armList is closest to the player.
    /// </summary>
    private ForgottenChocolateArmController GetClosestArmToPlayer()
    {
        ForgottenChocolateArmController closestArm = null;
        float closestDistance = float.MaxValue;

        foreach (ForgottenChocolateArmController arm in armList)
        {
            float distanceToPlayer = Vector3.Distance(arm.transform.position, player.position);
            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestArm = arm;
            }
        }

        return closestArm;
    }
}
