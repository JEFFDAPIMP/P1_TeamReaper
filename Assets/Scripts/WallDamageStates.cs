using UnityEngine;

[RequireComponent(typeof(Health))]
public class WallDamageStates : MonoBehaviour
{
    [SerializeField] private GameObject FirstObjectToRemove;
    [SerializeField] private GameObject SecondObjectToRemove;
    [SerializeField] private GameObject ThirdObjectToRemove;

    //reference to our health
    private Health health;

    // Start is called before the first frame update
    private void Start()
    {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (health.GetHealth() > 10)
        {
            FirstObjectToRemove.SetActive(true);
            SecondObjectToRemove.SetActive(true);
            ThirdObjectToRemove.SetActive(true);
        }

        if (0 < health.GetHealth() && health.GetHealth() <= 10)
        {
            FirstObjectToRemove.SetActive(false);
            SecondObjectToRemove.SetActive(true);
            ThirdObjectToRemove.SetActive(true);
        }

        if (0 < health.GetHealth() && health.GetHealth() <= 5)
        {
            FirstObjectToRemove.SetActive(false);
            SecondObjectToRemove.SetActive(false);
            ThirdObjectToRemove.SetActive(true);
        }

        if (health.GetHealth() <= 0)
        {
            FirstObjectToRemove.SetActive(false);
            SecondObjectToRemove.SetActive(false);
            ThirdObjectToRemove.SetActive(false);
        }
    }
}
