using UnityEngine;
using System;
using System.Globalization;
public static class AnimationSystemHandler
{
    #region Setting Methods
    public static void SetBool(Animator animator, string booleanName, bool value, AudioId? audioId, AnimationSoundMode mode, PlaybackForce force = PlaybackForce.Normal)
    {
        animator.SetBool(booleanName, value);
        if (audioId.HasValue)
        {
            AnimationSystemHandler.SetAudio(mode, (AudioId)audioId, force);
        }
    }
    public static void SetTrigger(Animator animator, string triggerName, AudioId? audioId, AnimationSoundMode mode, PlaybackForce force = PlaybackForce.Normal)
    {
        animator.SetTrigger(triggerName);
        if (audioId.HasValue)
        {
            AnimationSystemHandler.SetAudio(mode, (AudioId)audioId, force);
        }
    }
    public static void SetFloat(Animator animator, string floatName, float value, AudioId? audioId, AnimationSoundMode mode, PlaybackForce force = PlaybackForce.Normal)
    {
        animator.SetFloat(floatName, value);
        if (audioId.HasValue)
        {
            AnimationSystemHandler.SetAudio(mode, (AudioId)audioId, force);
        }
    }
    public static void SetInt(Animator animator, string intName, int value, AudioId? audioId, AnimationSoundMode mode, PlaybackForce force = PlaybackForce.Normal)
    {
        animator.SetInteger(intName, value);
        if (audioId.HasValue)
        {
            AnimationSystemHandler.SetAudio(mode, (AudioId)audioId, force);
        }
    }
    #endregion

    #region Audio Settings

    private static void SetAudio(AnimationSoundMode animationSoundMode, AudioId audioId, PlaybackForce force = PlaybackForce.Normal)
    {
        switch (animationSoundMode)
        {
            case AnimationSoundMode.On:
                AudioPlayer.Play(audioId, force);
                break;
            case AnimationSoundMode.Off:
                AudioPlayer.Stop(audioId);
                break;
        }

    }
    #endregion
}

public enum AnimationSoundMode
{
    On,
    Off
}