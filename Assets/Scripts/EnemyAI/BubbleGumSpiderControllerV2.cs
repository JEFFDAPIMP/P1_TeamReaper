using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BubbleGumSpiderControllerV2 : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private string playerTag = "Player";
    [SerializeField] private CaveManager caveManager;
    private string GameMasterTag = "GameMaster";
    [SerializeField] private NavMeshAgent navMeshAgent;

    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpDuration = 1f;
    [SerializeField] private float jumpDistance = 2f;
    private bool isJumping;

    [SerializeField] private GameObject chompProjectilePrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float chompProjectileSpeed = 1f;

    [SerializeField] private GameObject webProjectilePrefab;
    [SerializeField] private float webProjectileSpeed = 1f;

    private List<Transform> hidingSpots = new List<Transform>();
    private Transform bestHidingSpot = null;

    [SerializeField] private float spidersVisionRange = 100f;

    private Transform peekSpot1;
    private Transform peekSpot2;
    private Transform selectedPeekSpot;

    private float timeSinceLastShot = 0f;
    [SerializeField] private float shootInterval = 1f;

    public bool peekSpotChoosen = false;
    public bool isHidden = false;
    public bool inCover = false;
    public bool atPeekSpot = false;
    private bool movingToCover;

    [SerializeField] private GameObject stomper;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(playerTag);
        }

        if (caveManager == null)
        {
            caveManager = GameObject.FindGameObjectWithTag(GameMasterTag).GetComponent<CaveManager>();
        }

        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        navMeshAgent.updatePosition = true;
        navMeshAgent.updateRotation = true;

        stomper.SetActive(false);
    }

    void Update()
    {
        /*
         * Actions
         * 
         * If player is webbed, Then Jump at player and shoot player with chomp then start over.
         * Move to cover
         * Check if player can see me
         *      If Yes shoot player, Then start over
         *      If No step out and see if I can see player
         *          If Yes shoot player and move back to cover
         *          If No start over
         * 
         */

        if (player.GetComponent<FirstPersonController>().stuck)
        {
            if (!isJumping)
            {
                JumpPlayerActions();
            }
            //JumpPlayerActions();

            /*
            jumpPlayer();
            if (atDestination())
            {
                //ShootAtPlayer(chompProjectilePrefab, chompProjectileSpeed);
            }
            */
        }
        else
        {
            stomper.SetActive(false);
            navMeshAgent.enabled = true;
            if (isHidden)
            {
                InCoverActions();
            }
            else
            {
                HideActions();
            }
        }
    }

    private void JumpPlayerActions()
    {
        Debug.Log("BubbleGumSpiderController - player is stuck");
        peekSpotChoosen = false;
        isHidden = false;
        inCover = false;
        atPeekSpot = false;
        navMeshAgent.enabled = false;
        stomper.SetActive(true);
        JumpToPlayer();
    }

    private void HideActions()
    {
        if(movingToCover == false)
        {
            Debug.Log("BubbleGumSpiderController - HideActions - Looking for and moving to random cover");
            IdentifyCover();
            MoveToRandomCover();
            //MoveToCoverFurthestFromPlayer();
        }
        else
        {
            Debug.Log("BubbleGumSpiderController - HideActions - Cover found, waiting to arrive");
        }

        if (atDestination())
        {
            movingToCover = false;
            Debug.Log("BubbleGumSpiderController - HideActions - atDestination");

            if (CheckIfPlayerCanSeeMe())
            {
                Debug.Log("BubbleGumSpiderController - HideActions - atDestination - player can see me, shoot player then run loop again.");
                //ShootWithDelay(webProjectilePrefab, webProjectileSpeed);
                ShootAtPlayer(webProjectilePrefab, webProjectileSpeed);
            }
            else
            {
                isHidden = true;
                inCover = true;
            }
        }
    }

    /// <summary>
    /// Can player see us? if yes we are not hidden anymore
    /// </summary>
    private void InCoverActions()
    {
        Debug.Log("BubbleGumSpiderController - InCoverActions");

        
        if (!atDestination())
        {
            // Im not at my destination so repeat loop
            Debug.Log("BubbleGumSpiderController - InCoverActions - Still moving to destination");
            return;
        }

        if (inCover)
        {
            Debug.Log("BubbleGumSpiderController - InCoverActions - inCover");

            if (CheckIfPlayerCanSeeMe())
            {
                Debug.Log("Spotted! Shooting at player then setting isHidden to false and restarting loop");
                //ShootWithDelay(webProjectilePrefab, webProjectileSpeed);
                ShootAtPlayer(webProjectilePrefab, webProjectileSpeed);
                isHidden = false;
                return;
            }

            if (!peekSpotChoosen)
            {
                Debug.Log("BubbleGumSpiderController - InCoverActions - Choosing Peek Spot and moving there");

                GetPeekSpots();
                ChooseRandomPeekSpot();
                MoveToPeekSpot();

                peekSpotChoosen = true;
                inCover = false;
            }
        }
        else
        {
            Debug.Log("BubbleGumSpiderController - InCoverActions - At peek spot");

            if (CheckIfPlayerCanSeeMe())
            {
                Debug.Log("BubbleGumSpiderController - InCoverActions - Can see player, aiming");
                if (ShootWithDelay(webProjectilePrefab, webProjectileSpeed))
                {
                    Debug.Log("BubbleGumSpiderController - InCoverActions - SHOTS FIRED!");
                    navMeshAgent.SetDestination(bestHidingSpot.position);
                    peekSpotChoosen = false;
                    inCover = true;
                }
            }
            else
            {
                Debug.Log("BubbleGumSpiderController - InCoverActions - Cant get a shot, time to move!");
                //lets move to a different spot where we can shoot the player, we do this by setting all our triggers to false
                peekSpotChoosen = false;
                isHidden = false;
                inCover = false;
                atPeekSpot = false;
            }

        }
    }

    private bool ShootWithDelay(GameObject projectile, float projectileSpeed)
    {
        Debug.Log("Aiming");
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= shootInterval)
        {
            Debug.Log("FIRING A SHOT!");
            ShootAtPlayer(projectile, projectileSpeed);
            timeSinceLastShot = 0f;
            return true;
        }
        return false;
    }

    private void IdentifyCover()
    {
        VisionCheck[] visionCheck = caveManager.GetAllAvalibleHidingSpots();
        hidingSpots.Clear(); // Clear previous hiding spots
        foreach (VisionCheck currentVisionCheck in visionCheck)
        {
            if (!currentVisionCheck.CanSeePlayer())
            {
                hidingSpots.Add(currentVisionCheck.gameObject.transform);
            }
        }
    }

    private void MoveToRandomCover()
    {
        bestHidingSpot = null; // Reset best hiding spot

        bestHidingSpot = hidingSpots[Random.Range(0, hidingSpots.Count)];

        navMeshAgent.SetDestination(bestHidingSpot.position);

        movingToCover = true;
    }

    private void MoveToCoverFurthestFromPlayer()
    {
        bestHidingSpot = null; // Reset best hiding spot
        foreach (Transform currentHidingSpot in hidingSpots)
        {
            if (bestHidingSpot == null || Vector3.Distance(player.transform.position, bestHidingSpot.position) < Vector3.Distance(player.transform.position, currentHidingSpot.transform.position))
            {
                bestHidingSpot = currentHidingSpot;
            }
        }
        if (bestHidingSpot != null)
        {
            navMeshAgent.SetDestination(bestHidingSpot.position);
        }
    }

    private bool CheckIfPlayerCanSeeMe()
    {
        Ray ray = new Ray(transform.position, player.transform.position - transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, spidersVisionRange))
        {
            if (hit.collider.gameObject.tag == playerTag)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    private bool atDestination()
    {
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            Debug.Log("atDestination - true");
            return true;
        }
        Debug.Log("atDestination - false");
        return false;
    }

    private bool CoverStillExists()
    {
        if(bestHidingSpot == null || peekSpot1 == null || peekSpot2 == null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// get peek out spots to shoot from
    /// </summary>
    private void GetPeekSpots()
    {
        if(CoverStillExists())
        {
            peekSpot1 = bestHidingSpot.parent.transform.Find("PeekSpot");
            peekSpot2 = bestHidingSpot.parent.transform.Find("PeekSpot (1)");
        }
        else
        {
            IdentifyCover();
            MoveToRandomCover();
        }
    }

    private void ChooseRandomPeekSpot()
    {
        if (CoverStillExists())
        {
            selectedPeekSpot = Random.Range(0, 1) > 0.5f ? peekSpot1 : peekSpot2;
        }
        else
        {
            IdentifyCover();
            MoveToRandomCover();
        }
    }

    private void MoveToPeekSpot()
    {
        if (CoverStillExists())
        {
            navMeshAgent.SetDestination(selectedPeekSpot.position);
        }
        else
        {
            IdentifyCover();
            MoveToRandomCover();
        }
    }

    private void ShootAtPlayer(GameObject projectileType, float projectileSpeed)
    {
        // Instantiate the projectile at the shooting point
        GameObject projectile = Object.Instantiate(projectileType, shootingPoint.position, shootingPoint.rotation);

        // Calculate the direction to the player with offset of 1 from the ground
        Vector3 directionToPlayer = ((player.transform.position + new Vector3(0,1,0))- shootingPoint.position).normalized;

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
    private void JumpToPlayer()
    {
        if (!isJumping && Vector3.Distance(transform.position, player.transform.position) > jumpDistance)
        {
            StartCoroutine(JumpToPlayerCoroutine());
        }
    }

    IEnumerator JumpToPlayerCoroutine()
    {
        Debug.Log("JumpToPlayerCoroutine");
        isJumping = true;
        //navMeshAgent.enabled = false;

        Vector3 startPosition = transform.position;
        //Vector3 endPosition = player.transform.position + (transform.position - player.transform.position).normalized * jumpDistance;
        Vector3 endPosition = new Vector3(
                                    player.transform.position.x,
                                    player.transform.position.y + 1,  // Offset Y position
                                    player.transform.position.z
                                ) + (transform.position - player.transform.position).normalized * jumpDistance;

        float elapsedTime = 0f;

        Debug.Log("JumpToPlayerCoroutine - before loop");
        while (elapsedTime < jumpDuration)
        {
            Debug.Log("JumpToPlayerCoroutine - inside loop");
            float t = elapsedTime / jumpDuration;
            float height = Mathf.Sin(Mathf.PI * t) * jumpHeight;
            transform.position = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * height;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("JumpToPlayerCoroutine - after loop");

        transform.position = endPosition;
        //navMeshAgent.enabled = true;
        isJumping = false;
    }
}
