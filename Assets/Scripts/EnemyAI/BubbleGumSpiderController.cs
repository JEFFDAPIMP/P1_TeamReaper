using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// I need help with setting up a Bubble Gum Spider Boss
/// Create a script called BubbleGumSpiderController
/// 1) It needs to reference a CaveManager to identify spots to hide
/// 2) Move to the spot and check to see if the player can see spider boss if yes go back to step 1
/// 3) Step out of cover
/// 4) Shoot player with web
/// 5) return to cover
/// 
/// If player is stuck, jump at the player and shoot
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class BubbleGumSpiderController : MonoBehaviour
{
    //Variables for external objects I need to reference
    [SerializeField] private GameObject player; // Used to see if player can see me in cover and 
    private string playerTag = "Player"; // Because I forget to set variables in the inpector
    [SerializeField] private CaveManager caveManager; //Used to find potential cover points
    private string GameMasterTag = "GameMaster"; // Because I forget to set variables in the inpector

    [SerializeField] private NavMeshAgent navMeshAgent;

    //variables for jumping at player
    [SerializeField] private float jumpHeight = 5f; // Height of the jump
    [SerializeField] private float jumpDuration = 1f; // Duration of the jump
    [SerializeField] private float jumpDistance = 2f; // Distance from the player to land
    private bool isJumping;

    // Variables for chomp projectile
    [SerializeField] private GameObject chompProjectilePrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float chompProjectileSpeed = 1f;

    // Variables for web projectile
    [SerializeField] private GameObject webProjectilePrefab;
    [SerializeField] private float webProjectileSpeed = 1f;

    //For finding hiding spots
    private List<Transform> hidingSpots = new List<Transform>();
    private Transform bestHidingSpot = null;

    //length of raycast to check for player to see if player can see me
    [SerializeField] private float spidersVisionRange = 100f;


    //variables for shoot points from cover
    private Transform peekSpot1;
    private Transform peekSpot2;
    private Transform selectedPeekSpot;

    //varibles to help controller figure out what its doing
    public bool isHidden = false;
    public bool inCover = false;
    private float timeSinceLastShot;
    [SerializeField] private float shootInterval = 1f;


    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(playerTag);
        }

        if(caveManager == null)
        {
            caveManager = GameObject.FindGameObjectWithTag(GameMasterTag).GetComponent<CaveManager>();
        }

        if(navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<FirstPersonController>().stuck)
        {
            Debug.Log("BubbleGumSpiderController - player is stuck");
            isHidden = false;

            jumpPlayer();
            if (atDestination())
            {
                //ShootAtPlayer(chompProjectilePrefab, chompProjectileSpeed);
            }
            return;
        }
        if (isHidden)
        {
            InCoverActions();
        }
        else
        {
            HideActions();
        }
    }

    private void JumpPlayerActions()
    {
        jumpPlayer();
    }

    private void HideActions()
    {
        Debug.Log("BubbleGumSpiderController - HideActions");
        // Identify cover
        IdentifyCover();

        // Move to cover furthest from player
        MoveToCoverFurthestFromPlayer();


        // Once I get to cover continue
        if (atDestination())
        {
            Debug.Log("BubbleGumSpiderController - HideActions - atDestination");
            //check if player can see me
            if (CheckIfPlayerCanSeeMe())
            {
                Debug.Log("BubbleGumSpiderController - HideActions - atDestination - player can see me");
                HideActions();
                return;
            }
            isHidden = true;
        }
    }

    private void InCoverActions()
    {
        Debug.Log("BubbleGumSpiderController - InCoverActions");
        if (inCover)
        {
            Debug.Log("BubbleGumSpiderController - InCoverActions - inCover");
            //Im in cover, can I be seen?
            if (CheckIfPlayerCanSeeMe())
            {
                Debug.Log("Spotted!");
                isHidden = false;
                return;
            }

            //Nope? lets shoot the player.
            //Move To Peek Spot
            GetPeekSpots();
            ChooseRandomPeekSpot();
            MoveToPeekSpot();

            //Once I get there shoot the player
            if (atDestination())
            {
                ShootWithDelay(webProjectilePrefab, webProjectileSpeed);
                inCover = false;
                MoveToCover();
            }
        }
        else
        {
            Debug.Log("BubbleGumSpiderController - InCoverActions - Returning to cover");
            MoveToCover();
            if (atDestination())
            {
                Debug.Log("BubbleGumSpiderController - InCoverActions - arrived at cover");
                inCover = true;
            }
        }
    }

    private void ShootWithDelay(GameObject projectile, float projectileSpeed)
    {
        // Increment the timer by the time elapsed since the last frame
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= shootInterval)
        {
            ShootAtPlayer(projectile, projectileSpeed);
            timeSinceLastShot = 0f;
        }
    }

    /// <summary>
    /// Look for hinding spots, get all hiding spots from the cave manager then make sure there's no line of site from them to the player.
    /// </summary>
    private void IdentifyCover()
    {
        VisionCheck[] visionCheck = caveManager.GetAllAvalibleHidingSpots();
        foreach(VisionCheck currentVisionCheck in visionCheck)
        {
            if (!currentVisionCheck.CanSeePlayer())
            {
                hidingSpots.Add(currentVisionCheck.gameObject.transform);
            }
        }
    }

    /// <summary>
    /// Tell nav mesh agent to hide from player
    /// </summary>
    private void MoveToCoverFurthestFromPlayer()
    {
        foreach(Transform currentHidingSpot in hidingSpots)
        {
            //first found hiding spot is best one.
            if(bestHidingSpot == null)
            {
                bestHidingSpot = currentHidingSpot;
                continue;
            }

            //if best hiding spot is closer to player than current spot update
            if(Vector3.Distance(player.transform.position, bestHidingSpot.position) < Vector3.Distance(player.transform.position, currentHidingSpot.transform.position))
            {
                bestHidingSpot = currentHidingSpot;
            }
        }
        navMeshAgent.SetDestination(bestHidingSpot.position);
    }

    /// <summary>
    /// Once Im at the hiding spot check to see if the player can see me
    /// </summary>
    private bool CheckIfPlayerCanSeeMe()
    {
        // Create a ray starting from the camera's position and pointing forward
        Ray ray = new Ray(transform.position, player.transform.position);

        // Variable to store the hit information
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, spidersVisionRange))
        {
            // Check if the ray hits the player, if it does return true
            //Debug.Log("Hit: " + hit.collider.gameObject.name);
            if(hit.collider.gameObject == player)
            {
                MoveToCoverFurthestFromPlayer();
                return true;
            }
            return false;

            // Optionally, draw a debug line in the scene view
            //Debug.DrawLine(ray.origin, hit.point, Color.red);
        }
        else
        {
            // If the ray does not hit anything, draw the ray in green
            //Debug.DrawLine(ray.origin, ray.origin + ray.direction * spidersVisionRange, Color.green);
            return false;
        }
    }

    /// <summary>
    /// Check if Nav mesh is done
    /// </summary>
    /// <returns> if at destination and done moving return true, else false </returns>
    private bool atDestination()
    {
        //if (!navMeshAgent.pathPending)
        //{
            //Debug.Log("atDestination - pathPending - false");
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                //if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                //{
                    return true;
                //}
            }
        //}
        Debug.Log("atDestination - false");
        return false;
    }

    /// <summary>
    /// get peek out spots to shoot from
    /// </summary>
    private void GetPeekSpots()
    {
        peekSpot1 = bestHidingSpot.parent.transform.Find("PeekSpot");
        peekSpot2 = bestHidingSpot.parent.transform.Find("PeekSpot (1)");
    }

    private void ChooseRandomPeekSpot()
    {
        selectedPeekSpot = Random.Range(0,1) > 0.5f ? peekSpot1 : peekSpot2;
    }

    private void MoveToPeekSpot()
    {
        navMeshAgent.SetDestination(selectedPeekSpot.position);
    }

    private void ShootAtPlayer(GameObject projectileType, float projectileSpeed)
    {
        // Instantiate the projectile at the shooting point
        GameObject projectile = Object.Instantiate(projectileType, shootingPoint.position, shootingPoint.rotation);

        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.transform.position - shootingPoint.position).normalized;

        // Get the Rigidbody component of the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Apply force to the Rigidbody to move it towards the player using a scalar speed
        rb.AddForce(directionToPlayer * projectileSpeed, ForceMode.VelocityChange);
    }

    private void MoveToCover()
    {
        navMeshAgent.SetDestination(bestHidingSpot.position);
    }

    /// <summary>
    /// Jump to player if we are not already jumping and far enough to start jumping
    /// </summary>
    private void jumpPlayer()
    {
        if (!isJumping && Vector3.Distance(transform.position, player.transform.position) > jumpDistance)
        {
            StartCoroutine(JumpToPlayerCoroutine());
        }
    }

    IEnumerator JumpToPlayerCoroutine()
    {
        isJumping = true;
        navMeshAgent.enabled = false;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = player.transform.position + (transform.position - player.transform.position).normalized * jumpDistance;
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float height = Mathf.Sin(Mathf.PI * t) * jumpHeight;
            transform.position = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * height;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        navMeshAgent.enabled = true;
        isJumping = false;
    }


}