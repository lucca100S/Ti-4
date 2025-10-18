/// <summary>How playback requests are interpreted when audio is already playing.</summary>
public enum PlaybackForce
{
    Normal,         // Play normally (multiple instances allowed)
    ForceRestart,   // Stop existing and start new
    IgnoreIfPlaying // If at least one instance is playing, ignore the request
}
