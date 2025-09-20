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

    public LiquidJumpState(LiquidoState parent, PlayerStateMachine player, SurfaceDetection surface)
    {
        this.parent = parent;
        this.player = player;
        this.surface = surface;
    }

    public void Enter()
    {
        Debug.Log("[LiquidJump] Enter");
        player.AddJump(player.LiquidJump);
        jumpExecuted = true;
    }

    public void Update()
    {
        float h = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(h, 0f, 0f) * (player.LiquidSpeed * 0.8f);
        player.SetMovement(move);

        if (surface.CurrentSurface.HasValue &&
            surface.CurrentSurface.Value.type == SurfaceType.Floor &&
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
}
#endregion
