using UnityEngine;

public class DynamicSpriteSortingOrder : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;

    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100) + spriteRenderer.sortingOrder;
        }
    }
}