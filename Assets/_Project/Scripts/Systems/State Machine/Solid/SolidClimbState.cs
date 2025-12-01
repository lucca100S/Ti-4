using Systems.Input;
using Unity.VisualScripting;
using UnityEngine;

#region Substates - Solid - Climb
/// <summary>Escalada em S�lido.</summary>
public class SolidClimbState : IState
{
    private SolidoState parent;
    private SurfaceDetection surface;
    private PlayerStateMachine player;

    private bool _didStart = false;
    private bool _isPlayingClimbingSFX = false;

    public StateType StateType => StateType.Climb;

    public SolidClimbState(SolidoState parent, SurfaceDetection surface)
    {
        this.parent = parent;
        this.surface = surface;
        this.player = UnityEngine.Object.FindFirstObjectByType<PlayerStateMachine>();
    }

    public void Enter()
    {
        Debug.Log("[SolidClimb] Enter");
        AudioPlayer.Stop(AudioId.SolidStep);

        player.DidJump = false;
    }

    public void Update()
    {
        switch (player.PlayerController.CurrentMaterial)
        {
            case SurfaceMaterial.Vines:
                player.AddJump(1f);
                player.SetGravityDirection(-surface.CurrentSurface.Value.hit.normal);

                Vector3 move = player.DirectionInputClimb * (player != null ? player.SolidSpeed * 0.6f : 3f);
                player?.SetVelocity(move);

                if (move != Vector3.zero && !_isPlayingClimbingSFX)
                {
                    AudioPlayer.Play(AudioId.SolidStep);
                    _isPlayingClimbingSFX = true;
                }
                else if (move == Vector3.zero && _isPlayingClimbingSFX)
                {
                    AudioPlayer.Stop(AudioId.SolidStep);
                    _isPlayingClimbingSFX = false;
                }

                if (!_didStart)
                {
                    _didStart = true;
                    player.SetVelocity(Vector3.zero);
                }
                break;
            case SurfaceMaterial.Earth:
                player.AddJump(player.gravity * 0.1f);
                player.SetGravityDirection(Vector3.up);
                break;
            case SurfaceMaterial.Stone:
                player.SetGravityDirection(Vector3.zero);
                player.AddJump(0);

                if (!_didStart)
                {
                    _didStart = true;
                    player.SetVelocity(Vector3.zero);
                }
                break;
        }
    }

    public void Exit()
    {
        Debug.Log("[SolidClimb] Exit");
        _didStart = false;
    }

    public void OnJumpInput(InputInfo input)
    {

    }
}
#endregion
