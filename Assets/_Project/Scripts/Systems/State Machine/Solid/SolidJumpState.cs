using Systems.Input;
using UnityEngine;

#region Substates - Solid - Jump
/// <summary>Pulo no modo S�lido.</summary>
public class SolidJumpState : IState
{
    private SolidoState parent;
    private SurfaceDetection surface;
    private PlayerStateMachine player;

    private bool _didJump = false;

    public SolidJumpState(SolidoState parent, SurfaceDetection surface)
    {
        this.parent = parent;
        this.surface = surface;
        this.player = UnityEngine.Object.FindFirstObjectByType<PlayerStateMachine>();
    }

    public void Enter()
    {
        Debug.Log("[SolidJump] Enter");
        if (player.CanJump)
        {
            player?.AddJump(player.SolidJump);
            _didJump = true;
        }
        player.SetGravityDirection(Vector3.up);
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

        // quando tocar ch�o novamente, voltar para Idle/Walk (macro decide isso)
        if (player.IsGrounded)
        {
            Debug.Log("[SolidJump] Detectado ch�o -> transi��o ser� feita pela macro S�lido.");
        }
    }

    public void Exit()
    {
        _didJump = false;
        Debug.Log("[SolidJump] Exit");
    }

    public void OnJumpInput(InputInfo input)
    {
        if (_didJump && input.IsUp)
        {
            if (!player.IsGoingDown)
            {
                player.AddJump(player.VerticalVelocity.magnitude * 0.5f);
                AudioPlayer.Play(AudioRegistry.Instance.Get(AudioId.Jump));
                Debug.Log("[SolidJump] Jump Cancel");
            }
            _didJump = false;
        }
    }
}
#endregion
