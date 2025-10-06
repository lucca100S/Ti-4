using Player;
using System;
using Systems.Input;
using UnityEngine;

#region Player - State Machine (Root)
/// <summary>
/// Controlador principal do player. Mantém a máquina de macroestados e gerencia movimento físico.
/// - Integra SurfaceDetection (injeção via inspector)
/// - Possui CharacterController para mover o personagem.
/// - Permite que subestados controlem movimento via SetMovement / AddJump.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerStateMachine : MonoBehaviour
{
    #region Inspector Fields
    [Header("Dependencies")]
    [SerializeField] private SurfaceDetection surfaceDetection;

    [Header("Movement Settings")]
    [Tooltip("Velocidade no modo sólido (unidades/s)")]
    [SerializeField] public float solidMoveSpeed = 6f;
    [Tooltip("Velocidade no modo líquido (unidades/s)")]
    [SerializeField] public float liquidMoveSpeed = 4f;
    [Tooltip("Força de pulo sólido")]
    [SerializeField] public float solidJumpForce = 8f;
    [Tooltip("Força de pulo líquido")]
    [SerializeField] public float liquidJumpForce = 6f;
    [Tooltip("Gravidade aplicada ao player (negativo)")]
    [SerializeField] public float gravity = -20f;
    #endregion

    #region Internal State
    private CharacterController controller;
    private PlayerController playerController;
    private StateMachine macroStateMachine;
    private SolidoState solidoState;
    private LiquidoState liquidoState;

    // Movement composition (os subestados definem este vetor cada frame)
    private Vector3 accumulatedHorizontalMovement = Vector3.zero;
    private float verticalVelocity = 0f;
    private const float terminalVelocity = -50f;

    #endregion

    private Vector3 _directionInput = Vector3.zero;
    private InputInfo _jumpInput = new InputInfo { };
    private InputInfo _transformInput = new InputInfo { };
    private Vector3 _currentVelocity = Vector3.zero;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _deceleration = 10f;

    #region Unity Callbacks
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        controller = GetComponent<CharacterController>();
        macroStateMachine = new StateMachine();

        // criar macros e passar dependências
        solidoState = new SolidoState(this, surfaceDetection);
        liquidoState = new LiquidoState(this, surfaceDetection);
    }

    private void Start()
    {
        macroStateMachine.ChangeState(solidoState);
    }

    private void Update()
    {
        // Gravity & movement commit (reseta accumulated horizontal a cada frame)
        ApplyGravity();

        // Update state machine (substates vão chamar SetMovement / AddJump conforme necessário)
        macroStateMachine.Update();

        // Move CharacterController com o movimento acumulado
        Vector3 totalMove = transform.forward * accumulatedHorizontalMovement.z +
            transform.right * accumulatedHorizontalMovement.x +
            transform.up * accumulatedHorizontalMovement.y;

        _currentVelocity = Vector3.MoveTowards(_currentVelocity, totalMove, (totalMove.magnitude > _currentVelocity.magnitude ? _acceleration : _deceleration) * Time.deltaTime);

        controller.Move((_currentVelocity + (Vector3.up * verticalVelocity)) * Time.deltaTime);

        // Reset horizontal for next frame (vertical is persistent)
        accumulatedHorizontalMovement = Vector3.zero;
    }
    #endregion

    #region Movement API for States
    /// <summary>
    /// Chamado pelos subestados para mover horizontalmente (local X,Z).
    /// </summary>
    public void SetMovement(Vector3 worldMovement)
    {
        accumulatedHorizontalMovement += worldMovement;
    }

    /// <summary>
    /// Define a velocidade vertical (ex.: ao pular).
    /// </summary>
    public void AddJump(float jumpForce)
    {
        verticalVelocity = jumpForce;
        Debug.Log($"[Player] Jump applied: {jumpForce}");
    }
    #endregion

    #region Macro State Control
    private void ToggleMacroState()
    {
        if (macroStateMachine.CurrentState == solidoState)
        {
            macroStateMachine.ChangeState(liquidoState);
        }
        else
        {
            macroStateMachine.ChangeState(solidoState);
        }
    }

    /// <summary>Permite a outros scripts consultarem o macroestado atual.</summary>
    public bool IsInSolido => macroStateMachine.CurrentState == solidoState;
    public bool IsInLiquido => macroStateMachine.CurrentState == liquidoState;

    /// <summary>Exposição para possíveis usos (logs/inspector).</summary>
    public string CurrentMacroName => macroStateMachine.CurrentState?.GetType().Name ?? "None";
    #endregion

    private void ApplyGravity()
    {
        // Se está sobre piso detectável e indo para baixo, zera vertical
        if (surfaceDetection != null && surfaceDetection.CurrentSurface.HasValue &&
            surfaceDetection.CurrentSurface.Value.type == SurfaceType.Floor && verticalVelocity <= 0f)
        {
            // Mantém levemente negativo para garantir contato com CharacterController
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
            if (verticalVelocity < terminalVelocity)
                verticalVelocity = terminalVelocity;
        }
    }

    #region Inputs
    internal void GetDirectionInput(Vector3 direction)
    {
        _directionInput = direction;
    }

    internal void GetJumpInput(InputInfo info)
    {
        _jumpInput = info;
        macroStateMachine.JumpInput(info);
    }

    internal void GetTransformInput(InputInfo info)
    {
        _transformInput = info;
        if (info.IsDown)
        {
            ToggleMacroState();
        }
    }

    #endregion

    #region Accessors (para estados)
    public SurfaceDetection SurfaceDetection => surfaceDetection;
    public CharacterController CharacterController => controller;
    public PlayerController PlayerController => playerController;
    public float SolidSpeed => solidMoveSpeed;
    public float LiquidSpeed => liquidMoveSpeed;
    public float SolidJump => solidJumpForce;
    public float LiquidJump => liquidJumpForce;
    public float LastTimeOnGround => playerController.LastTimeOnGround;
    public bool IsGrounded => (surfaceDetection.CurrentSurface.HasValue && surfaceDetection.CurrentSurface.Value.type == SurfaceType.Floor);
    public Vector3 CurrentVelocity => controller.velocity;
    public Vector3 DirectionInput
    {
        get {
            Vector3 direction = _directionInput;
            direction = Camera.main.transform.forward * direction.z +
                    Camera.main.transform.right * direction.x;
            direction.y = 0;
            return direction.normalized;
        }
    }
    public Vector3 DirectionInputClimb
    {
        get
        {
            Vector3 up = _directionInput.z * Orientation.up;
            Vector3 right = _directionInput.x * Orientation.right;
            return (up + right).normalized;
        }
    }
    public InputInfo JumpInput => _jumpInput;
    public InputInfo TransformInput => _transformInput;
    public Transform Orientation => playerController.Orientation;

    public bool CanJump { get { return JumpInput.GetDelayInput(LastTimeOnGround) || IsGrounded; } private set { } }
    #endregion
}
#endregion
