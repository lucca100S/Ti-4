using Systems.Input;
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

    Vector3 _normalDirection = Vector3.forward;

    public LiquidIdleState IdleState { get; private set; }
    public LiquidWalkState WalkState { get; private set; }
    public LiquidJumpState JumpState { get; private set; }
    public LiquidWallJumpState WallJumpState { get; private set; }
    public Vector3 NormalDirection => _normalDirection;
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
        AudioPlayer.Play(AudioRegistry.Instance.Get(AudioId.Liquid));
        player.GetComponent<Animator>().SetBool("Liquid", true);
        player.GetComponent<Animator>().SetBool("OnIdle", true);
        subStateMachine.ChangeState(IdleState);
    }

    public void Update()
    {
        if (surface.CurrentSurface.HasValue)
        {
            

            switch (surface.CurrentSurface.Value.type)
            {
                case SurfaceType.Wall:
                case SurfaceType.Floor:
                case SurfaceType.Ceiling:
                    Debug.Log("IsGoingDown here: " + player.IsGoingDown);
                    if (player.IsGoingDown)
                    {
                        Vector3 dir = player.DirectionInputNormal;
                        Debug.Log("Dir here: " + dir);
                        if (dir.magnitude > 0.01f)
                        {
                            subStateMachine.ChangeState(WalkState);
                        }
                        else
                        {
                            subStateMachine.ChangeState(IdleState);
                        }
                    }
                    break;

                default:
                    subStateMachine.ChangeState(IdleState);
                    break;
            }

            Vector3 normal = surface.CurrentSurface.Value.hit.normal;
            Vector3 moveDir = player.DirectionInputNormal;
            _normalDirection = moveDir;

            if (moveDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir, normal);
                player.PlayerController.RotateModelTowards(targetRotation);
            }
            else
            {
                // Sem input, apenas alinhamento com a superfície
                Quaternion alignRotation = Quaternion.FromToRotation(player.transform.up, normal) * player.transform.rotation;
                player.PlayerController.RotateModelTowards(alignRotation);
            }
            
            switch(player.PlayerController.CurrentMaterial)
            {
                case SurfaceMaterial.Earth:
                    player.SetGravityDirection(normal);
                    break;
                case SurfaceMaterial.Stone:
                    player.SetGravityDirection(Vector3.up);
                    break;
            }

        }
        else
        {
            player.SetGravityDirection(Vector3.up);
            // fallback: ficar em jump enquanto no ar
            subStateMachine.ChangeState(JumpState);
        }

        subStateMachine.Update();
    }

    public void Exit()
    {
        Debug.Log("[Macro] Saiu de Líquido");
        player.GetComponent<Animator>().SetBool("Liquid", false);
        player.SetGravityDirection(Vector3.up);
    }
    #endregion

    #region Utilities para subestados
    public void ChangeSubState(IState newState) => subStateMachine.ChangeState(newState);

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
                    if (input.IsDown)
                        subStateMachine.ChangeState(WallJumpState);
                    break;
                case SurfaceType.Ceiling:

                    break;
            }
        }

        subStateMachine.CurrentState?.OnJumpInput(input);

    }
    #endregion
}
#endregion
