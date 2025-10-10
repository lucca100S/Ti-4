using Systems.Input;
using UnityEngine;

#region Substates - Solid - Climb
/// <summary>Escalada em Sólido.</summary>
public class SolidClimbState : IState
{
    private SolidoState parent;
    private SurfaceDetection surface;
    private PlayerStateMachine player;

    public SolidClimbState(SolidoState parent, SurfaceDetection surface)
    {
        this.parent = parent;
        this.surface = surface;
        this.player = UnityEngine.Object.FindFirstObjectByType<PlayerStateMachine>();
    }

    public void Enter() => Debug.Log("[SolidClimb] Enter");

    public void Update()
    {
        switch (player.PlayerController.CurrentMaterial)
        {
            case SurfaceMaterial.Vines:
                player.AddJump(0);
                player.SetGravityDirection(Vector3.zero);
                if (player.DirectionInput != Vector3.zero)
                {
                    Vector3 move = player.DirectionInputClimb * (player != null ? player.SolidSpeed * 0.6f : 3f);
                    player?.SetMovement(move);
                }
                break;
            case SurfaceMaterial.Earth:
                player.AddJump(player.gravity * 0.1f);
                player.SetGravityDirection(Vector3.up);
                break;
            case SurfaceMaterial.Stone:
                player.SetGravityDirection(Vector3.zero);
                break;
        }
    }

    public void Exit() => Debug.Log("[SolidClimb] Exit");

    public void OnJumpInput(InputInfo input)
    {
        
    }
}
#endregion
