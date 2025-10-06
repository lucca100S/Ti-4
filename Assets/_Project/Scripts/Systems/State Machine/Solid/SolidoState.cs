using Systems.Input;
using UnityEngine;

#region Macros - Solido
/// <summary>
/// Macroestado S�lido � cont�m subm�quina para Idle / Walk / Jump / WallJump / Climb.
/// Decide subestados com base no SurfaceDetection e inputs.
/// </summary>
public class SolidoState : IState
{
    #region Fields
    private PlayerStateMachine player;
    private SurfaceDetection surface;
    private StateMachine subStateMachine;

    // Inst�ncias de subestados
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

        // criar subestados e passar refer�ncia da macro/Surface
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
        Debug.Log("[Macro] Entrou em S�lido");
        subStateMachine.ChangeState(IdleState);
    }

    public void Update()
    {

        // Decide transi��es prim�rias por superf�cie + input
        if (surface.CurrentSurface.HasValue)
        {
            switch (surface.CurrentSurface.Value.type)
            {
                case SurfaceType.Floor:
                    // andar/idle/pular
                    if (player.CurrentVelocity.y <= 0)
                    {
                        Vector3 dir = player.DirectionInput;
                        if (dir.magnitude > 0.01f)
                        {
                            player.PlayerController.RotateModelTowards(dir);
                            subStateMachine.ChangeState(WalkState);
                        }
                        else
                        {
                            subStateMachine.ChangeState(IdleState);
                        }
                    }
                    break;

                case SurfaceType.Wall:
                    Vector3 hitNormal = surface.CurrentSurface.Value.hit.normal;
                    // escalar e walljump
                    float DOTProduct = Vector3.Dot(player.DirectionInput.normalized, -hitNormal);
                    if (DOTProduct > 0.8f)
                        subStateMachine.ChangeState(ClimbState);

                    player.PlayerController.RotateModelTowards(-hitNormal);
                    break;

                case SurfaceType.Ceiling:
                    // sem comportamento espec�fico por enquanto
                    subStateMachine.ChangeState(IdleState);
                    break;
            }
        }
        else
        {
            if (player.DirectionInput != Vector3.zero)
            {
                player.PlayerController.RotateModelTowards(player.DirectionInput);
            }
            // Em "ar" sem superf�cie detect�vel � deixar um fallback
            subStateMachine.ChangeState(JumpState);
        }

        subStateMachine.Update();
    }

    public void Exit()
    {
        Debug.Log("[Macro] Saiu de S�lido");
    }

    public void OnJumpInput(InputInfo input)
    {
        if (surface.CurrentSurface.HasValue)
        {
            switch (surface.CurrentSurface.Value.type)
            {
                case SurfaceType.Floor:
                    if (input.IsDown)
                        subStateMachine.ChangeState(JumpState);
                    break;
                case SurfaceType.Wall:
                    if(input.IsDown)
                        subStateMachine.ChangeState(WallJumpState);
                    break;
                case SurfaceType.Ceiling:

                    break;
            }
        }
    }
    #endregion

    #region Utilities para subestados
    public void ChangeSubState(IState newState) => subStateMachine.ChangeState(newState);

    
    #endregion
}
#endregion
