using UnityEngine;

#region Substates - Liquid - Walk
/// <summary>
/// Movimento horizontal no modo Líquido.
/// Possui atrito menor e velocidade diferente da forma sólida.
/// </summary>
public class LiquidWalkState : IState
{
    private readonly LiquidoState parent;
    private readonly PlayerStateMachine player;
    private readonly SurfaceDetection surface;

    public LiquidWalkState(LiquidoState parent, PlayerStateMachine player, SurfaceDetection surface)
    {
        this.parent = parent;
        this.player = player;
        this.surface = surface;
    }

    public void Enter()
    {
        Debug.Log("[LiquidWalk] Enter");
    }

    public void Update()
    {
        float h = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(h, 0f, 0f) * player.LiquidSpeed;
        player.SetMovement(move);
    }

    public void Exit()
    {
        Debug.Log("[LiquidWalk] Exit");
    }
}
#endregion
