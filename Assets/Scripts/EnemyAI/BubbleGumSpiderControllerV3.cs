using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BubbleGumSpiderControllerV3 : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private string playerTag = "Player";
    [SerializeField] private CaveManager caveManager;
    private string gameMasterTag = "GameMaster";
    [SerializeField] private NavMeshAgent navMeshAgent;

    [SerializeField] private GameObject regularProjectilePrefab;
    [SerializeField] private GameObject specialProjectilePrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float regularProjectileSpeed = 10f;
    [SerializeField] private float specialProjectileSpeed = 10f;
    [SerializeField] private float shootInterval = 1f;

    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpDuration = 1f;
    [SerializeField] private float jumpDistance = 2f;
    private bool isJumping;

    private List<Transform> hidingSpots = new List<Transform>();
    private Transform currentHidingSpot = null;
    private Transform peekSpot1;
    private Transform peekSpot2;
    private Transform selectedPeekSpot;

    private float timeSinceLastShot = 0f;
    private bool isHidden = false;
    private bool inCover = false;
    private bool peekSpotChosen = false;
    private float spidersVisionRange = 100f;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(playerTag);
        }

        if (caveManager == null)
        {
            caveManager = GameObject.FindGameObjectWithTag(gameMasterTag).GetComponent<CaveManager>();
        }

        if (navMeshAgent == null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        navMeshAgent.updatePosition = true;
        navMeshAgent.updateRotation = true;
        IdentifyCover();
    }

    private void Update()
    {
        /*
        if (player.GetComponent<FirstPersonController>().stuck)
        {
            if (!isJumping)
            {
                StartCoroutine(JumpAtPlayer());
            }
        }
        else
        {
            if (isHidden)
            {
                InCoverActions();
            }
            else
            {
                HideActions();
            }
        }
        */
        HideActions();
    }

    private void HideActions()
    {
        IdentifyCover();

        if (hidingSpots.Count > 0)
        {
            MoveToRandomCover();
        }
        else
        {
            Debug.LogWarning("No hiding spots available!");
        }

        if (AtDestination())
        {
            if (!CheckIfPlayerCanSeeMe())
            {
                isHidden = true;
                inCover = true;
            }
        }
    }

    private void InCoverActions()
    {
        if (!AtDestination())
        {
            return;
        }

        if (inCover)
        {
            if (!CheckIfPlayerCanSeeMe())
            {
                if (!peekSpotChosen)
                {
                    GetPeekSpots();
                    ChooseRandomPeekSpot();
                    if (selectedPeekSpot != null)
                    {
                        MoveToPeekSpot();
                        peekSpotChosen = true;
                        inCover = false;
                    }
                    else
                    {
                        Debug.LogWarning("No valid peek spot found!");
                        isHidden = false;
                    }
                }
            }
            else
            {
                isHidden = false;
            }
        }
        else
        {
            if (CheckIfPlayerCanSeeMe())
            {
                if (ShootWithDelay(regularProjectilePrefab, regularProjectileSpeed))
                {
                    MoveToCover();
                    peekSpotChosen = false;
                    inCover = true;
                }
            }
            else
            {
                isHidden = false;
            }
        }
    }

    private bool ShootWithDelay(GameObject projectile, float projectileSpeed)
    {
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= shootInterval)
        {
            ShootAtPlayer(projectile, projectileSpeed);
            timeSinceLastShot = 0f;
            return true;
        }
        return false;
    }

    private void IdentifyCover()
    {
        VisionCheck[] visionChecks = caveManager.GetAllAvalibleHidingSpots();
        hidingSpots.Clear();
        foreach (VisionCheck visionCheck in visionChecks)
        {
            if (!visionCheck.CanSeePlayer())
            {
                hidingSpots.Add(visionCheck.transform);
            }
        }
    }

    private void MoveToRandomCover()
    {
        currentHidingSpot = hidingSpots[Random.Range(0, hidingSpots.Count)];
        navMeshAgent.SetDestination(currentHidingSpot.position);
    }

    private void GetPeekSpots()
    {
        peekSpot1 = currentHidingSpot.Find("PeekSpot1");
        peekSpot2 = currentHidingSpot.Find("PeekSpot2");
    }

    private void ChooseRandomPeekSpot()
    {
        selectedPeekSpot = (Random.Range(0, 2) == 0) ? peekSpot1 : peekSpot2;
    }

    private void MoveToPeekSpot()
    {
        if (selectedPeekSpot != null)
        {
            navMeshAgent.SetDestination(selectedPeekSpot.position);
        }
        else
        {
            Debug.LogWarning("No valid peek spot selected!");
        }
    }

    private void MoveToCover()
    {
        if (currentHidingSpot != null)
        {
            navMeshAgent.SetDestination(currentHidingSpot.position);
        }
    }

    private void ShootAtPlayer(GameObject projectileType, float projectileSpeed)
    {
        GameObject projectile = Instantiate(projectileType, shootingPoint.position, shootingPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        Vector3 directionToPlayer = (player.transform.position - shootingPoint.position).normalized;
        rb.velocity = directionToPlayer * projectileSpeed;
    }

    private bool CheckIfPlayerCanSeeMe()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, directionToPlayer);
        if (Physics.Raycast(ray, out RaycastHit hit, spidersVisionRange))
        {
            return hit.collider.gameObject.CompareTag(playerTag);
        }
        return false;
    }

    private bool AtDestination()
    {
        return !navMeshAgent.pathPending &&
               navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance &&
               (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 0.01f);
    }

    private IEnumerator JumpAtPlayer()
    {
        isJumping = true;
        navMeshAgent.enabled = false;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = player.transform.position + (transform.position - player.transform.position).normalized * jumpDistance;
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float height = Mathf.Sin(Mathf.PI * t) * jumpHeight;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t) + Vector3.up * height;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        navMeshAgent.enabled = true;
        isJumping = false;

        // Shoot a special projectile at the player
        ShootAtPlayer(specialProjectilePrefab, specialProjectileSpeed);
    }
}
