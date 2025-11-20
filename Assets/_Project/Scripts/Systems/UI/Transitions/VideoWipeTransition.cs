using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System;

public class VideoWipeTransition : IUITransition
{
    private GameObject transitionPanel;      // panel with VideoPlayer
    private VideoPlayer videoPlayer;
    private MonoBehaviour runner;
    private float hideAfter = 0.05f;         // threshold for backward finish

    public VideoWipeTransition(GameObject transitionPanel, VideoPlayer videoPlayer, MonoBehaviour runner)
    {
        this.transitionPanel = transitionPanel;
        this.videoPlayer = videoPlayer;
        this.runner = runner;

        transitionPanel.SetActive(false);
        videoPlayer.isLooping = false;
        videoPlayer.playOnAwake = false;
    }

    public void PlayEnter(Panel originPanel, Panel targetPanel, System.Action onComplete)
    {
        runner.StartCoroutine(RunTransition(originPanel, targetPanel, onComplete));
    }

    public void PlayEnter(Panel panel, Action onComplete = null)
    {
        throw new NotImplementedException();
    }

    // (Used only if you want transitions on exiting a state)
    public void PlayExit(Panel panel, System.Action onComplete = null)
    {
        panel.gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    private IEnumerator RunTransition(Panel origin, Panel target, System.Action onComplete)
    {
        // 1. Enable transition panel (VIDEO OVER EVERYTHING)
        transitionPanel.SetActive(true);

        // Make sure origin is visible, target is hidden.
        origin.gameObject.SetActive(true);
        target.gameObject.SetActive(false);

        // 2. Play video FORWARD
        videoPlayer.time = 0;
        videoPlayer.playbackSpeed = 1f;
        videoPlayer.Play();

        // Wait for forward playback to end
        while (videoPlayer.isPlaying)
            yield return null;

        // 3. Mid-point: swap panels
        origin.gameObject.SetActive(false);
        target.gameObject.SetActive(true);

        // 4. Play video BACKWARD to reveal target
        videoPlayer.playbackSpeed = -1f;
        videoPlayer.Play();

        // Wait until near beginning
        while (videoPlayer.time > hideAfter)
            yield return null;

        videoPlayer.Stop();

        // 5. Hide transition panel
        transitionPanel.SetActive(false);

        // 6. Done
        onComplete?.Invoke();
    }
}
