using Systems.Input;
using UnityEngine;

#region Substates - Solid - Jump
/// <summary>Pulo no modo Sólido.</summary>
public class SolidJumpState : IState
{
    private SolidoState parent;
    private SurfaceDetection surface;
    private PlayerStateMachine player;

    public SolidJumpState(SolidoState parent, SurfaceDetection surface)
    {
        this.parent = parent;
        this.surface = surface;
        this.player = UnityEngine.Object.FindFirstObjectByType<PlayerStateMachine>();
    }

    public void Enter()
    {
        Debug.Log("[SolidJump] Enter");
        AudioPlayer.Stop(AudioId.SolidStep);
        if (player.CanJump)
        {
            player.GetComponent<Animator>().SetTrigger("Jump");
            AudioPlayer.Play(AudioId.SolidJump);
            player?.AddJump(player.SolidJump);
            player.DidJump = true;
        }
        else if(parent.LastState == parent.WallJumpState)
        {
            player.DidJump = true;
        }

        if (player.DidJump)
        {
            player.LastJumpInputOnGround = -Mathf.Infinity;
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

        // quando tocar chão novamente, voltar para Idle/Walk (macro decide isso)
        if (player.IsGrounded)
        {
            Debug.Log("[SolidJump] Detectado chão -> transição será feita pela macro Sólido.");
        }
    }

    public void Exit()
    {
        player.DidJump = false;
        Debug.Log("[SolidJump] Exit");
        
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
            player.DidJump = false;
        }
        else if(player.CanJump && input.IsDown && !player.DidJump)
        {
            player?.AddJump(player.SolidJump);
            player.DidJump = true;
        }
    }
}
#endregion
