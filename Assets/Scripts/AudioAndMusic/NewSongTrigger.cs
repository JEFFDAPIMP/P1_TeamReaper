using UnityEngine;

public class NewSongTrigger : MonoBehaviour
{
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private string tag = "Player";
    [SerializeField] private int newtrackNumber;
    [SerializeField] private int oldtrackNumber;
    [SerializeField] private float volume;

    private void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        /*
        if (other.CompareTag(tag))
        {
            musicManager.ChangeTrack(newtrackNumber, volume); // Instantly set the second track to 50% volume
            //musicManager.ChangeTrackWithTransition(newtrackNumber, volume); // Instantly set the second track to 50% volume
            //musicManager.StopAndChangeTrackAfterLoop(newtrackNumber, volume); // Instantly set the second track to 50% volume
            musicManager.StopTrack(oldtrackNumber); // Instantly stop the first track
        }
        */

        /**/
        if (other.CompareTag("Player"))
        {
            MusicManager2 musicManager2 = FindObjectOfType<MusicManager2>();
            musicManager2.AddTrack(newtrackNumber, volume); // Change to the second track with a transition
            musicManager2.StopTrack(oldtrackNumber);
        }
        /**/
    }


}
