using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager3 : MonoBehaviour
{
    public double musicDuration;
    public double goalTime;
    public double delayStart = 0.5;

    public AudioClip currentClip;
    public AudioSource[] _audioSources;
    public int audioToggle;

    private void OnPlayMusic()
    {
        goalTime = AudioSettings.dspTime + delayStart;

        _audioSources[audioToggle].clip = currentClip;
        _audioSources[audioToggle].PlayScheduled(goalTime);

        musicDuration = (double)currentClip.samples / currentClip.frequency;
        goalTime = goalTime + musicDuration;
    }

    private void Update()
    {
        if(AudioSettings.dspTime > goalTime - 1)
        {
            PlayScheduledClip();
        }
    }

    private void PlayScheduledClip()
    {
        _audioSources[audioToggle].clip = currentClip;
        _audioSources[audioToggle].PlayScheduled(goalTime);

        musicDuration = (double)currentClip.samples / currentClip.frequency;
        goalTime = AudioSettings.dspTime + musicDuration;

        audioToggle = 1 - audioToggle;
    }

    public void SetCurrentClip(AudioClip audioClip)
    {
        currentClip = audioClip;
    }
}
