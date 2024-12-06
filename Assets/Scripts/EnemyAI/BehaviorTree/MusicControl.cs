using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;

public class MusicControl : Action
{
    public MusicManager musicManager;
    public SharedInt trackToStop;
    public SharedInt trackToStart;
    public SharedFloat volume;
    public SharedBool fade;
    public SharedFloat fadeDuration;
    public override TaskStatus OnUpdate()
    {
        if (fade.Value)
        {
            musicManager.FadeInTrack(trackToStart.Value, volume.Value, fadeDuration.Value);
            musicManager.FadeOutTrack(trackToStop.Value, trackToStop.Value);
        }
        else {
            musicManager.ChangeTrack(trackToStart.Value, volume.Value); // Instantly set the second track to 50% volume
            musicManager.StopTrack(trackToStop.Value); // Instantly stop the first track
        }
        return TaskStatus.Success;
    }
}
