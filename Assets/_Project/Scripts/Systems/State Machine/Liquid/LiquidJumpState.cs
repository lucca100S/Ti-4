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

    private bool _didJump = false;

    public StateType StateType => StateType.Jump;

    public LiquidJumpState(LiquidoState parent, PlayerStateMachine player, SurfaceDetection surface)
    {
        this.parent = parent;
        this.player = player;
        this.surface = surface;
    }

    public void Enter()
    {
        _didJump = player.DidJump;

        _originalDirection = player.DirectionInput;
        Debug.Log("[LiquidJump] Enter");
        if (player.CanJump)
        {
            player.AddJump(player.LiquidJump);
            player.DidJump = true;
        }

        _didJump = player.DidJump && !_didJump;

        if(_didJump)
        {
            AudioPlayer.Play(AudioId.SolidJump);
            ActionsManager.Instance.OnPlayerJumped?.Invoke();
        }
    }

    public void Update()
    {
        Vector3 move = _originalDirection * player.LiquidSpeed + player.DirectionInput * (player.LiquidSpeed * 0.2f);
        move *= 1.15f;
        
        if (_didJump)
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
        ActionsManager.Instance.OnPlayerLanded?.Invoke();
    }

    public void OnJumpInput(InputInfo input)
    {
        if (_didJump && input.IsUp)
        {
            if (!player.IsGoingDown)
            {
                player.AddJump(player.VerticalVelocity.magnitude * 0.5f);
                Debug.Log("[LiquidJump] Jump Cancel");
            }
            _didJump = false;
        }
        else if (player.CanJump && input.IsDown && !player.DidJump)
        {
            player?.AddJump(player.LiquidJump);
            _didJump = true;
        }
    }
}
#endregion
