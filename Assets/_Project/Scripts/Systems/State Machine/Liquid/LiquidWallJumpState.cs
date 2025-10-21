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

        if (!executed && player.IsGrounded)
        {
            Vector3 normal = parent.NormalDirection;
            Vector3 push = (normal).normalized;
            player.AddJump(player.LiquidJump);
            Debug.Log($"[LiquidWallJump] Executado com push {push}");
            executed = true;
        }

        if(executed)
        {
            Vector3 move = player.DirectionInputNormal * player.LiquidSpeed;
            player.SetVelocity(move);
        }
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        Debug.Log("[LiquidWallJump] Exit");
    }

    public void OnJumpInput(InputInfo input)
    {

    }
}
#endregion
