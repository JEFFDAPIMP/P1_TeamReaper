using StarterAssets;
using UnityEngine;

public class TestingTools : MonoBehaviour
{
    public GameObject[] enemyList;

    public StarterAssetsInputs starterAssetsInputs;

    private void Update()
    {
        ReEnabledEnemies();
    }

    private void ReEnabledEnemies()
    {
        if (starterAssetsInputs.reload)
        {
            Debug.Log("reload pressed");
            foreach (GameObject enemy in enemyList)
            {
                enemy.GetComponent<Health>().Damage(-5, Health.allDamageType.Wet);
                enemy.SetActive(true);
            }
            starterAssetsInputs.reload = false;
        }
    }
}
