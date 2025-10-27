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
            player.DidJump = true;
        }

        if (player.DidJump)
        {
            ActionsManager.Instance.OnPlayerJumped?.Invoke();
            player.LastJumpInputOnGround = -Mathf.Infinity;
        }
    }

    public void Update()
    {
        Vector3 move = _originalDirection * player.LiquidSpeed + player.DirectionInput * (player.LiquidSpeed * 0.2f);
        if (player.DidJump)
        {
            player.SetVelocity(move);
        }

        if (_originalDirection != Vector3.zero && !player.SurfaceDetection.CurrentSurface.HasValue)
        {
            player.PlayerController.RotateModelTowards(move.normalized);
        }
    }

    public void Exit()
    {
        Debug.Log("[LiquidJump] Exit");
        player.DidJump = false;
        ActionsManager.Instance.OnPlayerLanded?.Invoke();
    }

    public void OnJumpInput(InputInfo input)
    {
        if (player.DidJump && input.IsUp)
        {
            if (!player.IsGoingDown)
            {
                player.AddJump(player.VerticalVelocity.magnitude * 0.5f);
                Debug.Log("[LiquidJump] Jump Cancel");
            }
            player.DidJump = false;
        }
        else if (player.CanJump && input.IsDown && !player.DidJump)
        {
            player?.AddJump(player.SolidJump);
            player.DidJump = true;
        }
    }
}
#endregion
