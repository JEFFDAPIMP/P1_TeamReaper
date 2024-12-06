using UnityEngine;

public class StopAllTracksTriggerFade : MonoBehaviour
{
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private string tag = "Player";
    [SerializeField] private float duration = 1.0f;

    private void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag))
        {
            musicManager.StopAllTracksExceptMainFade(duration);
        }
    }
}
