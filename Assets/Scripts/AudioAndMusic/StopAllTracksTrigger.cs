using UnityEngine;

public class StopAllTracksTrigger : MonoBehaviour
{
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private string tag = "Player";

    private void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag))
        {
            musicManager.StopAllTracksExceptMain();
        }
    }
}
