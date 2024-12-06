using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSongTriggerFade : MonoBehaviour
{
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private string tag = "Player";
    [SerializeField] private int fadeInTrackNumber;
    [SerializeField] private int fadeOutTrackNumber;
    [SerializeField] private float volume;
    [SerializeField] private float fadeDuration;

    private void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag))
        {
            Debug.Log("player");
            musicManager.FadeInTrack(fadeInTrackNumber, volume, fadeDuration);
            musicManager.FadeOutTrack(fadeOutTrackNumber, fadeDuration);
        }
    }
}