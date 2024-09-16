using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject BossHealthDisplay;
    [SerializeField] private Slider healthBarSlider;
    private Health health;

    void Start()
    {
        health = GetComponent<Health>();
        BossHealthDisplay.SetActive(true);
        healthBarSlider.maxValue = health.maxHealth;
        healthBarSlider.value = health.health;
    }

    void Update()
    {
        healthBarSlider.value = health.health;

        if(healthBarSlider.value <= 0)
        {
            BossHealthDisplay.SetActive(false);
        }
    }
}
