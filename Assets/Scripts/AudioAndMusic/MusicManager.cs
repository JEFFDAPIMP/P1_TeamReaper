using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public int mainTrack = 0;
    public float mainTrackVolume = 0.5f;

    public AudioClip transitionClip;
    private int currentTrackIndex = 0;

    public AudioClip[] musicTracks; // Array to hold different music tracks
    private List<AudioSource> audioSources = new List<AudioSource>();

    private AudioSource transitionSource;

    public GameObject[] monstersWithMusic; // Array to hold different music tracks

    public float previousTime;

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

        transitionSource = gameObject.AddComponent<AudioSource>();
        transitionSource.clip = transitionClip;
        transitionSource.loop = false;
        transitionSource.volume = 1;
        //transitionSource.Play();

        ChangeTrack(mainTrack, mainTrackVolume);
    }

    private void Update()
    {
        if (audioSources[mainTrack].time < previousTime)
        {
            previousTime = audioSources[mainTrack].time;
        }
    }

    public void ChangeTrack(int trackIndex, float targetVolume)
    {
        if (trackIndex < audioSources.Count)
        {
            audioSources[trackIndex].volume = targetVolume;
            currentTrackIndex = trackIndex;
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

    public void StopAllTracksExceptMainFade(float duration)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (i == mainTrack) { continue; }

            FadeOutTrack(i, duration);
        }
    }


    public void StopAndChangeTrackAfterLoop(int trackIndex, float targetVolume)
    {
        if (trackIndex < audioSources.Count)
        {
            StartCoroutine(DoStopAndChangeAudioAfterLoopTime(trackIndex, targetVolume));
        }
    }

    private IEnumerator DoStopAndChangeAudioAfterLoopTime(int trackIndex, float targetVolume)
    {
        yield return new WaitForSeconds(audioSources[mainTrack].time - previousTime);
        transitionSource.Play();
        StopTrack(currentTrackIndex);
        ChangeTrack(trackIndex, targetVolume);
    }

    /*
    
    public void ChangeTrackWithTransition(int newTrackIndex, float targetVolume)
    {
        if (newTrackIndex != currentTrackIndex && newTrackIndex < musicTracks.Length)
        {
            StartCoroutine(PlayTransitionAndChangeTrack(newTrackIndex, targetVolume));
        }
    }

    private IEnumerator PlayTransitionAndChangeTrack(int newTrackIndex, float targetVolume)
    {
        // Play the transition clip
        transitionSource.Play();
        yield return new WaitForSeconds(transitionClip.length);

        // Change to the new track
        currentTrackIndex = newTrackIndex;
        ChangeTrack(currentTrackIndex, targetVolume);
    }
    */

    public void FadeInTrack(int trackIndex, float targetVolume, float duration)
    {
        Debug.Log("Music Manager - inside FadeInTrack");
        if (trackIndex < audioSources.Count)
        {
            StartCoroutine(FadeInCoroutine(audioSources[trackIndex], targetVolume, duration));
        }
    }

    private IEnumerator FadeInCoroutine(AudioSource audioSource, float targetVolume, float duration)
    {
        Debug.Log("Music Manager - inside FadeInCoroutine");
        float startVolume = audioSource.volume;
        float time = 0;

        //audioSource.Play();

        while (time < duration)
        {
            Debug.Log("Music Manager - inside FadeInCoroutine - loop");
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
    public void FadeOutTrack(int trackIndex, float duration)
    {
        if (trackIndex < audioSources.Count)
        {
            StartCoroutine(FadeOutCoroutine(audioSources[trackIndex], duration));
        }
    }

    private IEnumerator FadeOutCoroutine(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float time = 0;

        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0;
        //audioSource.Stop();
    }

}
