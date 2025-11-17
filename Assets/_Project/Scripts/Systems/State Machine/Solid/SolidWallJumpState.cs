using Systems.Input;
using UnityEngine;

#region Substates - Solid - WallJump
/// <summary>Wall jump no modo S�lido.</summary>
public class SolidWallJumpState : IState
{
    private SolidoState parent;
    private SurfaceDetection surface;
    private PlayerStateMachine player;
    private bool executed = false;

    public StateType StateType => StateType.WallJump;

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
            // Impulso longe da parede: como n�o temos a normal do movimento aqui, usamos a normal do hit
            Vector3 normal = player.PlayerController.LastNormal;
            // aplicar jump com componente para cima
            player?.SetVelocity(normal * player.solidWallJumpForce);
            player.SetGravityDirection(Vector3.up);
            AudioPlayer.Play(AudioId.SolidJump);
            player?.AddJump(player.solidWallJumpHeight);
            Debug.Log($"[SolidWallJump] Executando walljump com push {normal}");
            
            executed = true;
        }
    }

    public void Update()
    {
    }

    public void Exit() 
    { 
        Debug.Log("[SolidWallJump] Exit");
        player.DidJump = false;
    }

    public void OnJumpInput(InputInfo input)
    {
        
    }
}
#endregion
