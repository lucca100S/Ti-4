using Systems.Input;
using UnityEngine;

#region Substates - Liquid - Jump
/// <summary>
/// Estado de pulo no modo Líquido.
/// Permite controle aéreo e reduz gravidade se necessário.
/// </summary>
public class LiquidJumpState : IState
{
    private readonly LiquidoState parent;
    private readonly PlayerStateMachine player;
    private readonly SurfaceDetection surface;
    private bool jumpExecuted;

    private Vector3 _originalDirection;

    public LiquidJumpState(LiquidoState parent, PlayerStateMachine player, SurfaceDetection surface)
    {
        this.parent = parent;
        this.player = player;
        this.surface = surface;
    }

    public void Enter()
    {
        _originalDirection = player.DirectionInput;
        Debug.Log("[LiquidJump] Enter");
        if (player.CanJump)
        {
            player.AddJump(player.LiquidJump);
            jumpExecuted = true;
        }
    }

    public void Update()
    {
        Vector3 move = _originalDirection * player.LiquidSpeed + player.DirectionInput * (player.LiquidSpeed * 0.2f);
        player.SetMovement(move);

        if (player.IsGrounded &&
            jumpExecuted)
        {
            Debug.Log("[LiquidJump] Detectado chão - macro fará transição.");
            jumpExecuted = false;
        }
    }

    public void Exit()
    {
        Debug.Log("[LiquidJump] Exit");
    }

    public void OnJumpInput(InputInfo input)
    {
        
    }
}
#endregion
