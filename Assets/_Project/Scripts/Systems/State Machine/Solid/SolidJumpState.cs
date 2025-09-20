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
        player?.AddJump(player.SolidJump);
    }

    public void Update()
    {
        // Durante o pulo, permitir controle horizontal reduzido
        float h = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(h, 0f, 0f) * (player != null ? player.SolidSpeed : 6f);
        player?.SetMovement(move);

        // quando tocar chão novamente, voltar para Idle/Walk (macro decide isso)
        if (surface.CurrentSurface.HasValue && surface.CurrentSurface.Value.type == SurfaceType.Floor)
        {
            Debug.Log("[SolidJump] Detectado chão -> transição será feita pela macro Sólido.");
        }
    }

    public void Exit() => Debug.Log("[SolidJump] Exit");
}
#endregion
