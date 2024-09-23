using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public int mainTrack = 0;
    public float mainTrackVolume = 0.5f;

    public AudioClip[] musicTracks; // Array to hold different music tracks
    private List<AudioSource> audioSources = new List<AudioSource>();

    public GameObject[] monstersWithMusic; // Array to hold different music tracks

    void Start()
    {
        foreach (var track in musicTracks)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = track;
            newSource.loop = true;
            newSource.volume = 0;
            audioSources.Add(newSource);
        }

        // Start all tracks at the same time
        foreach (var source in audioSources)
        {
            source.Play();
        }

        // Start all tracks at the same time
        foreach (var monster in monstersWithMusic)
        {
            monster.GetComponent<AudioSource>().Play();
        }

        ChangeTrack(mainTrack, mainTrackVolume);
    }

    public void ChangeTrack(int trackIndex, float targetVolume)
    {
        if (trackIndex < audioSources.Count)
        {
            audioSources[trackIndex].volume = targetVolume;
        }
    }

    public void StopTrack(int trackIndex)
    {
        if (trackIndex < audioSources.Count)
        {
            audioSources[trackIndex].volume = 0;
        }
    }

    public void StopAllTracksExceptMain()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (i == mainTrack) { continue; }
            audioSources[i].volume = 0;
        }
    }
}
