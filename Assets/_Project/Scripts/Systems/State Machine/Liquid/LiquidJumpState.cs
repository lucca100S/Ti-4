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
        jumpExecuted = false;

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
        player.SetGravityDirection(Vector3.up);

        Vector3 move = _originalDirection * player.LiquidSpeed + player.DirectionInput * (player.LiquidSpeed * 0.2f);
        if (jumpExecuted)
        {
            player.SetVelocity(move);
        }

        if (_originalDirection != Vector3.zero)
        {
            player.PlayerController.RotateModelTowards(move.normalized);
        }
    }

    public void Exit()
    {
        Debug.Log("[LiquidJump] Exit");
        jumpExecuted = false;
    }

    public void OnJumpInput(InputInfo input)
    {

    }
}
#endregion
