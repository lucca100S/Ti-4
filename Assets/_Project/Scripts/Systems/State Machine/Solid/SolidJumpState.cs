using Systems.Input;
using UnityEngine;

#region Substates - Solid - Jump
/// <summary>Pulo no modo Sólido.</summary>
public class SolidJumpState : IState
{
    private SolidoState parent;
    private SurfaceDetection surface;
    private PlayerStateMachine player;

    private bool _didJump = false;

    public StateType StateType => StateType.Jump;

    public SolidJumpState(SolidoState parent, SurfaceDetection surface)
    {
        this.parent = parent;
        this.surface = surface;
        this.player = UnityEngine.Object.FindFirstObjectByType<PlayerStateMachine>();
    }

    public void Enter()
    {
        _didJump = player.DidJump;

        Debug.Log("[SolidJump] Enter");
        AudioPlayer.Stop(AudioId.SolidStep);
        if (player.CanJump)
        {
            //player.GetComponent<Animator>().SetTrigger("Jump");
            
            player?.AddJump(player.SolidJump);

            player.DidJump = true;
        }
        else if (parent.LastState == parent.WallJumpState)
        {
            player.DidJump = true;
        }

        player.SetGravityDirection(Vector3.up);

        _didJump = player.DidJump && !_didJump;

        if(_didJump)
        {
            AudioPlayer.Play(AudioId.SolidJump);
            ActionsManager.Instance.OnPlayerJumped?.Invoke();
        }
    }

    public void Update()
    {
        // Durante o pulo, permitir controle horizontal reduzido
        Vector3 move = player.DirectionInput * (player != null ? player.SolidSpeed : 6f);
        player?.SetMovement(move);

        Vector3 lookDirection = player.CurrentVelocity;
        lookDirection.y = 0;
        lookDirection.Normalize();

        if (lookDirection != Vector3.zero)
        {
            player.PlayerController.RotateModelTowards(lookDirection);
        }

    }

    public void Exit()
    {
        Debug.Log("[SolidJump] Exit");

        if(player.IsGrounded)
            ActionsManager.Instance.OnPlayerLanded?.Invoke();
    }

    public void OnJumpInput(InputInfo input)
    {
        if (player.DidJump && input.IsUp)
        {
            if (!player.IsGoingDown)
            {
                player.AddJump(player.VerticalVelocity.magnitude * 0.5f);
                Debug.Log("[SolidJump] Jump Cancel");

            }
        }
        else if (player.CanJump && input.IsDown && !player.DidJump)
        {
            AudioPlayer.Play(AudioId.SolidJump);
            ActionsManager.Instance.OnPlayerJumped?.Invoke();
            player?.AddJump(player.SolidJump);
            player.DidJump = true;
        }
    }
}
#endregion
