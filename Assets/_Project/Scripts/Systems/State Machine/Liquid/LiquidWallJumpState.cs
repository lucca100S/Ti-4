using Systems.Input;
using UnityEngine;

#region Substates - Liquid - WallJump
/// <summary>
/// Wall jump no modo Líquido.
/// Empurra o jogador na direção oposta à parede detectada.
/// </summary>
public class LiquidWallJumpState : IState
{
    private readonly LiquidoState parent;
    private readonly PlayerStateMachine player;
    private readonly SurfaceDetection surface;
    private bool executed;

    public LiquidWallJumpState(LiquidoState parent, PlayerStateMachine player, SurfaceDetection surface)
    {
        this.parent = parent;
        this.player = player;
        this.surface = surface;
    }

    public void Enter()
    {
        Debug.Log("[LiquidWallJump] Enter");
        executed = false;
    }

    public void Update()
    {
        if (!executed && surface.CurrentSurface.HasValue && surface.CurrentSurface.Value.type == SurfaceType.Wall)
        {
            Vector3 normal = surface.CurrentSurface.Value.hit.normal;
            Vector3 push = (normal + Vector3.up * 0.6f).normalized;
            player.SetMovement(push * player.LiquidSpeed);
            player.AddJump(player.LiquidJump * 0.9f);
            Debug.Log($"[LiquidWallJump] Executado com push {push}");
            executed = true;
        }
    }

    public void Exit()
    {
        Debug.Log("[LiquidWallJump] Exit");
    }

    public void OnJumpInput(InputInfo input)
    {
        throw new System.NotImplementedException();
    }
}
#endregion
