using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayOneshotAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClipToPlay;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClipToPlay);
    }
}
