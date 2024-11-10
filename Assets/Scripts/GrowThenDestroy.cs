using UnityEngine;

public class GrowThenDestroy : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(2f, 2f, 2f);
    public float duration = 5f;

    private Vector3 initialScale;
    private float elapsedTime = 0f;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (elapsedTime < duration)
        {
            // Calculate the progress
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;

            // Interpolate the scale
            transform.localScale = Vector3.Lerp(initialScale, targetScale, progress);
        }
        else
        {
            // Destroy the object once the target scale is reached
            Destroy(gameObject);
        }
    }
}
