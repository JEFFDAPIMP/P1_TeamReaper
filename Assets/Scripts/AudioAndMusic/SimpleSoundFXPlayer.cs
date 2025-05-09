using UnityEngine;

// This can be added to objects that only need to loop one sound effect, example would be the fire wall
public class SimpleSoundFXPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip audioClip;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
