using Systems.Input;
using UnityEngine;

#region Substates - Solid - WallJump
/// <summary>Wall jump no modo Sólido.</summary>
public class SolidWallJumpState : IState
{
    private SolidoState parent;
    private SurfaceDetection surface;
    private PlayerStateMachine player;
    private bool executed = false;

    public SolidWallJumpState(SolidoState parent, SurfaceDetection surface)
    {
        this.parent = parent;
        this.surface = surface;
        this.player = UnityEngine.Object.FindFirstObjectByType<PlayerStateMachine>();
    }

    public void Enter()
    {
        Debug.Log("[SolidWallJump] Enter");
        executed = false;

        if (!executed)
        {
            Debug.Log("Executed");
            // Impulso longe da parede: como não temos a normal do movimento aqui, usamos a normal do hit
            if (surface.CurrentSurface.HasValue && surface.CurrentSurface.Value.type == SurfaceType.Wall)
            {
                Vector3 normal = surface.CurrentSurface.Value.hit.normal;
                // aplicar jump com componente para cima
                player?.SetVelocity(normal * player.solidWallJumpForce);
                player.SetGravityDirection(Vector3.up);
                player?.AddJump(player.solidWallJumpHeight);
                Debug.Log($"[SolidWallJump] Executando walljump com push {normal}");
            }
            executed = true;
        }
    }

    public void Update()
    {
    }

    public void Exit() => Debug.Log("[SolidWallJump] Exit");

    public void OnJumpInput(InputInfo input)
    {
        
    }
}
#endregion
