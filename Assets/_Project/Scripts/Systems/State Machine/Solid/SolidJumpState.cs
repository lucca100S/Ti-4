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
        if (player.CanJump)
        {
            player?.AddJump(player.SolidJump);
        }
    }

    public void Update()
    {
        // Durante o pulo, permitir controle horizontal reduzido
        Vector3 move = player.DirectionInput * (player != null ? player.SolidSpeed : 6f);
        player?.SetMovement(move);

        // quando tocar chão novamente, voltar para Idle/Walk (macro decide isso)
        if (player.IsGrounded)
        {
            Debug.Log("[SolidJump] Detectado chão -> transição será feita pela macro Sólido.");
        }
    }

    public void Exit() => Debug.Log("[SolidJump] Exit");

    public void OnJumpInput(InputInfo input)
    {
        
    }
}
#endregion
