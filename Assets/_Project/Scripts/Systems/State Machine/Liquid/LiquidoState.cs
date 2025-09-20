using UnityEngine;

#region Macros - Liquido
/// <summary>
/// Macroestado Líquido — contém subestados Idle / Walk / Jump / WallJump.
/// </summary>
public class LiquidoState : IState
{
    #region Fields
    private PlayerStateMachine player;
    private SurfaceDetection surface;
    private StateMachine subStateMachine;

    public LiquidIdleState IdleState { get; private set; }
    public LiquidWalkState WalkState { get; private set; }
    public LiquidJumpState JumpState { get; private set; }
    public LiquidWallJumpState WallJumpState { get; private set; }
    #endregion

    #region Constructor
    public LiquidoState(PlayerStateMachine player, SurfaceDetection surface)
    {
        this.player = player;
        this.surface = surface;

        subStateMachine = new StateMachine();

        IdleState = new LiquidIdleState(this, player, surface);
        WalkState = new LiquidWalkState(this, player, surface);
        JumpState = new LiquidJumpState(this, player, surface);
        WallJumpState = new LiquidWallJumpState(this, player, surface);
    }

    #endregion

    #region IState
    public void Enter()
    {
        Debug.Log("[Macro] Entrou em Líquido");
        subStateMachine.ChangeState(IdleState);
    }

    public void Update()
    {
        if (surface.CurrentSurface.HasValue)
        {
            switch (surface.CurrentSurface.Value.type)
            {
                case SurfaceType.Floor:
                    float h = Input.GetAxis("Horizontal");
                    if (Mathf.Abs(h) > 0.01f)
                        subStateMachine.ChangeState(WalkState);
                    else
                        subStateMachine.ChangeState(IdleState);

                    if (Input.GetKeyDown(KeyCode.Space))
                        subStateMachine.ChangeState(JumpState);
                    break;

                case SurfaceType.Wall:
                    if (Input.GetKeyDown(KeyCode.Space))
                        subStateMachine.ChangeState(WallJumpState);
                    break;

                default:
                    subStateMachine.ChangeState(IdleState);
                    break;
            }
        }
        else
        {
            // fallback: ficar em jump enquanto no ar
            subStateMachine.ChangeState(JumpState);
        }

        subStateMachine.Update();
    }

    public void Exit()
    {
        Debug.Log("[Macro] Saiu de Líquido");
    }
    #endregion

    #region Utilities para subestados
    public void ChangeSubState(IState newState) => subStateMachine.ChangeState(newState);
    #endregion
}
#endregion
