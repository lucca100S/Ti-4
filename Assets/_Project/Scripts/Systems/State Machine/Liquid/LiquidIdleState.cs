using Systems.Input;
using UnityEngine;

#region Substates - Liquid - Idle
/// <summary>
/// Estado Idle no modo Líquido.
/// Não aplica movimento, serve como estado "neutro".
/// </summary>
public class LiquidIdleState : IState
{
    private readonly LiquidoState parent;
    private readonly PlayerStateMachine player;
    private readonly SurfaceDetection surface;

    public LiquidIdleState(LiquidoState parent, PlayerStateMachine player, SurfaceDetection surface)
    {
        this.parent = parent;
        this.player = player;
        this.surface = surface;
    }

    public void Enter()
    {
        Debug.Log("[LiquidIdle] Enter");
        player.GetComponent<Animator>().SetBool("Liquid", false);
    }

    public void Update()
    {
        // Idle não faz nada além de aguardar inputs e mudanças de superfície
    }

    public void Exit()
    {
        Debug.Log("[LiquidIdle] Exit");
    }

    public void OnJumpInput(InputInfo input)
    {

    }
}
#endregion
