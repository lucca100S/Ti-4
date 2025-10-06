using UnityEngine;

#region Macros - Solido
/// <summary>
/// Macroestado Sólido — contém submáquina para Idle / Walk / Jump / WallJump / Climb.
/// Decide subestados com base no SurfaceDetection e inputs.
/// </summary>
public class SolidoState : IState
{
    #region Fields
    private PlayerStateMachine player;
    private SurfaceDetection surface;
    private StateMachine subStateMachine;

    // Instâncias de subestados
    public SolidIdleState IdleState { get; private set; }
    public SolidWalkState WalkState { get; private set; }
    public SolidJumpState JumpState { get; private set; }
    public SolidWallJumpState WallJumpState { get; private set; }
    public SolidClimbState ClimbState { get; private set; }
    #endregion

    #region Constructor
    public SolidoState(PlayerStateMachine player, SurfaceDetection surface)
    {
        this.player = player;
        this.surface = surface;

        subStateMachine = new StateMachine();

        // criar subestados e passar referência da macro/Surface
        IdleState = new SolidIdleState(this, surface);
        WalkState = new SolidWalkState(this, surface);
        JumpState = new SolidJumpState(this, surface);
        WallJumpState = new SolidWallJumpState(this, surface);
        ClimbState = new SolidClimbState(this, surface);
    }
    #endregion

    #region IState
    public void Enter()
    {
        Debug.Log("[Macro] Entrou em Sólido");
        subStateMachine.ChangeState(IdleState);
    }

    public void Update()
    {
        // Decide transições primárias por superfície + input
        if (surface.CurrentSurface.HasValue)
        {
            switch (surface.CurrentSurface.Value.type)
            {
                case SurfaceType.Floor:
                    // andar/idle/pular
                    float h = Input.GetAxis("Horizontal");
                    if (Mathf.Abs(h) > 0.01f)
                    {
                        subStateMachine.ChangeState(WalkState);
                    }
                    else
                    {
                        subStateMachine.ChangeState(IdleState);
                    }

                    if (Input.GetKeyDown(KeyCode.Space))
                        subStateMachine.ChangeState(JumpState);
                    break;

                case SurfaceType.Wall:
                    // escalar e walljump
                    if (Input.GetKey(KeyCode.W))
                        subStateMachine.ChangeState(ClimbState);

                    if (Input.GetKeyDown(KeyCode.Space))
                        subStateMachine.ChangeState(WallJumpState);
                    break;

                case SurfaceType.Ceiling:
                    // sem comportamento específico por enquanto
                    subStateMachine.ChangeState(IdleState);
                    break;
            }
        }
        else
        {
            // Em "ar" sem superfície detectável — deixar um fallback
            subStateMachine.ChangeState(JumpState);
        }

        subStateMachine.Update();
    }

    public void Exit()
    {
        Debug.Log("[Macro] Saiu de Sólido");
    }
    #endregion

    #region Utilities para subestados
    public void ChangeSubState(IState newState) => subStateMachine.ChangeState(newState);
    #endregion
}
#endregion
