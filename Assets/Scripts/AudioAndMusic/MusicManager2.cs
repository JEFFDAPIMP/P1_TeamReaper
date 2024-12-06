using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MusicManager2 : MonoBehaviour
{
    public AudioClip mainTrackClip;
    public AudioClip transitionClip;
    public AudioClip[] additionalTracks;

    private AudioSource mainTrackSource;
    private AudioSource transitionSource;
    private List<AudioSource> additionalTrackSources = new List<AudioSource>();

    private bool isTransitioning = false;

    void Start()
    {
        // Set up the main track
        mainTrackSource = gameObject.AddComponent<AudioSource>();
        mainTrackSource.clip = mainTrackClip;
        mainTrackSource.loop = false;
        mainTrackSource.volume = 1f;
        mainTrackSource.Play();

        // Set up the transition track
        transitionSource = gameObject.AddComponent<AudioSource>();
        transitionSource.clip = transitionClip;
        transitionSource.loop = false;
        transitionSource.volume = 1f;

        // Set up additional tracks
        foreach (var track in additionalTracks)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = track;
            newSource.loop = false;
            newSource.volume = 0;
            additionalTrackSources.Add(newSource);
        }
    }

    void Update()
    {
        //Debug.Log("mainTrackSource.time = " + mainTrackSource.time + ", mainTrackSource.clip.length = " + mainTrackSource.clip.length + ", mainTrackSource.clip.length - Time.deltaTime = " + (mainTrackSource.clip.length - Time.deltaTime));
        //Debug.Log("mainTrackSource.time = " + mainTrackSource.time + ", mainTrackSource.clip.length = " + mainTrackSource.clip.length + ", mainTrackSource.clip.length - Time.deltaTime = " + (mainTrackSource.clip.length - Time.deltaTime));
        Debug.Log("mainTrackSource.isPlaying = " + mainTrackSource.isPlaying);
        // Check if the main track has finished its loop
        //mainTrackSource.isPlaying
        //if (mainTrackSource.time >= mainTrackSource.clip.length - Time.deltaTime)
        if (!mainTrackSource.isPlaying)
        {
            //Debug.Log("condition 1 is true");
            //if (!isTransitioning)
            //{
                Debug.Log("condition 2 is true");
                //StartCoroutine(PlayTransitionAndHandleTracks());
                dothething();
            //}
        }
    }

    private void dothething()
    {
        //isTransitioning = true;

        // Play the transition clip
        transitionSource.Play();
        // Restart the main track
        mainTrackSource.Play();

        //yield return new WaitForSeconds(transitionClip.length);

        // Handle additional tracks
        foreach (var source in additionalTrackSources)
        {
            if (source.volume > 0)
            {
                source.Play();
            }
        }

        //isTransitioning = false;
    }

    private IEnumerator PlayTransitionAndHandleTracks()
    {
        isTransitioning = true;

        // Play the transition clip
        transitionSource.Play();
        // Restart the main track
        mainTrackSource.Play();

        //yield return new WaitForSeconds(transitionClip.length);

        // Handle additional tracks
        foreach (var source in additionalTrackSources)
        {
            if (source.volume > 0)
            {
                source.Play();
            }
        }

        yield return new WaitForSeconds(transitionClip.length);

        isTransitioning = false;
    }

    public void AddTrack(int trackIndex, float targetVolume)
    {
        if (trackIndex < additionalTrackSources.Count)
        {
            additionalTrackSources[trackIndex].volume = targetVolume;
        }
    }

    public void StopTrack(int trackIndex)
    {
        if (trackIndex < additionalTrackSources.Count)
        {
            //additionalTrackSources[trackIndex].volume = 0;
            //additionalTrackSources[trackIndex].Stop();
            StartCoroutine(TransitionToStop(trackIndex));
        }
    }

    private IEnumerator TransitionToStop(int trackIndex)
    {
        float lengthOfTracks = 12f;
        yield return new WaitForSeconds(lengthOfTracks - additionalTrackSources[trackIndex].time);
        additionalTrackSources[trackIndex].volume = 0;
    }
}
