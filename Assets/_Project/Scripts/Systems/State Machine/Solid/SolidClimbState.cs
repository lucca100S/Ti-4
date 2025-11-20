using Systems.Input;
using UnityEditor;
using UnityEngine;

#region Substates - Solid - Climb
/// <summary>Escalada em Sólido.</summary>
public class SolidClimbState : IState
{
    private SolidoState parent;
    private SurfaceDetection surface;
    private PlayerStateMachine player;

    private bool _didStart = false;

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

        EditorApplication.isPaused = true;
    }

    public void OnJumpInput(InputInfo input)
    {

    }
}
#endregion
