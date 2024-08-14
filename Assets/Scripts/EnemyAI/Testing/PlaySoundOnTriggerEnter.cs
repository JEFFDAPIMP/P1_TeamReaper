using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class PlaySoundOnTriggerEnter : MonoBehaviour
{
    public GameObject Target;
    public AudioClip[] audioClips;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomClip();
    }

    public void PlayRandomClip()
    {
        if (audioClips.Length > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Length);
            AudioClip clipToPlay = audioClips[randomIndex];
            audioSource.PlayOneShot(clipToPlay);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayRandomClip();
    }

    private void OnTriggerStay(Collider other)
    {
        PlayRandomClip();
    }
}
