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
public class PlayerStateMachine : MonoBehaviour
{
    #region Inspector Fields
    [Header("Dependencies")]
    [SerializeField] private SurfaceDetection surfaceDetection;

    [Header("Movement Settings")]
    [Header("Solid Speeds")]
    [Tooltip("Velocidade no modo sólido (unidades/s)")]
    [SerializeField] private float solidMoveSpeedVines = 6f;
    [SerializeField] private float solidMoveSpeedStone = 6f;
    [SerializeField] private float solidMoveSpeedEarth = 6f;

    [Header("Liquid Speeds")]
    [Tooltip("Velocidade no modo líquido (unidades/s)")]
    [SerializeField] private float liquidMoveSpeedVines = 6f;
    [SerializeField] private float liquidMoveSpeedStone = 6f;
    [SerializeField] private float liquidMoveSpeedEarth = 6f;

    [Header("Jumping")]
    [Tooltip("Força de pulo sólido")]
    [SerializeField] public float solidJumpForce = 8f;
    [SerializeField] public float solidWallJumpForce = 8f;
    [SerializeField] public float solidWallJumpHeight = 2f;
    [Tooltip("Força de pulo líquido")]
    [SerializeField] public float liquidJumpForce = 6f;
    [SerializeField] public float liquidWallJumpForce = 8f;

    [Tooltip("Gravidade aplicada ao player (negativo)")]
    [SerializeField] public float gravity = -20f;
    #endregion

    #region Internal State
    private Rigidbody _rigidBody;
    private PlayerController playerController;
    private StateMachine macroStateMachine;
    private SolidoState solidoState;
    private LiquidoState liquidoState;

    // Movement composition (os subestados definem este vetor cada frame)
    private Vector3 accumulatedHorizontalMovement = Vector3.zero;
    private const float terminalVelocity = -50f;
    private Vector3 _gravityDirection = Vector3.up;
    private Vector3 verticalVelocity = Vector3.up;

    #endregion

    private Vector3 _directionInput = Vector3.zero;
    private InputInfo _jumpInput = new InputInfo { };
    private InputInfo _transformInput = new InputInfo { };
    private Vector3 _currentVelocity = Vector3.zero;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _deceleration = 10f;
    [SerializeField] private float _airDeceleration = 10f;
    private float _lastJumpInputOnGround = -Mathf.Infinity;
    private bool _didJump = false;

    #region Unity Callbacks
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        _rigidBody = GetComponentInChildren<Rigidbody>();
        macroStateMachine = new StateMachine();

        // criar macros e passar dependências
        solidoState = new SolidoState(this, surfaceDetection);
        liquidoState = new LiquidoState(this, surfaceDetection);

        macroStateMachine.ChangeState(solidoState);
    }

    private void Update()
    {
        // Gravity & movement commit (reseta accumulated horizontal a cada frame)
        ApplyGravity();

        // Update state machine (substates vão chamar SetMovement / AddJump conforme necessário)
        macroStateMachine.Update();

        // Move CharacterController com o movimento acumulado
        Vector3 totalMove = accumulatedHorizontalMovement;

        _currentVelocity = Vector3.MoveTowards(_currentVelocity, totalMove, (totalMove.magnitude > _currentVelocity.magnitude ? _acceleration : IsGrounded ? _deceleration : _airDeceleration) * Time.deltaTime);

        _rigidBody.linearVelocity = (_currentVelocity + verticalVelocity);

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
    public void SetVelocity(Vector3 worldVelocity)
    {
        _currentVelocity = worldVelocity;
    }

    public void SetGravityDirection(Vector3 direction)
    {
        _gravityDirection = direction.normalized;
    }

    /// <summary>
    /// Define a velocidade vertical (ex.: ao pular).
    /// </summary>
    public void AddJump(float jumpForce)
    {
        verticalVelocity = jumpForce * _gravityDirection;
        Debug.Log($"[Player] Jump applied: {jumpForce}");
    }
    public void AddJump(float jumpForce, Vector3 direction)
    {
        verticalVelocity = jumpForce * direction;
        Debug.Log($"[Player] Jump applied: {jumpForce}");
    }
    #endregion

    #region Macro State Control
    private void ToggleMacroState()
    {
        this.GetComponent<Animator>().SetBool("KeepAtState", false);
        if (macroStateMachine.CurrentState == solidoState)
        {
            macroStateMachine.ChangeState(liquidoState);
        }
        else
        {
            macroStateMachine.ChangeState(solidoState);
        }

        ActionsManager.Instance.OnFormChanged?.Invoke(macroStateMachine.CurrentState);
    }

    /// <summary>Permite a outros scripts consultarem o macroestado atual.</summary>
    public bool IsInSolido => macroStateMachine.CurrentState == solidoState;
    public bool IsInLiquido => macroStateMachine.CurrentState == liquidoState;

    /// <summary>Exposição para possíveis usos (logs/inspector).</summary>
    public string CurrentMacroName => macroStateMachine.CurrentState?.GetType().Name ?? "None";
    #endregion

    internal void ApplyGravity()
    {
        float magnitude = verticalVelocity.magnitude;
        Vector3 directionChange = Vector3.Lerp(VerticalVelocity.normalized, -_gravityDirection.normalized, Time.deltaTime * 2).normalized;

        if (CoyoteTime && !IsGrounded && IsGoingDown && !_didJump)
        {
            verticalVelocity = Vector3.zero;
            return;
        }

        verticalVelocity = directionChange * magnitude;
        // Se está sobre piso detectável e indo para baixo, zera vertical
        if (IsGrounded && IsGoingDown)
        {
            // Mantém levemente negativo para garantir contato com CharacterController
            verticalVelocity = -2f * _gravityDirection;
        }
        else
        {
            verticalVelocity += gravity * _gravityDirection * Time.deltaTime;
            if (verticalVelocity.magnitude < terminalVelocity)
                verticalVelocity = verticalVelocity.normalized * terminalVelocity;
        }
    }

    public void ResetForces()
    {
        verticalVelocity = Vector3.zero;
        _currentVelocity = Vector3.zero;
        accumulatedHorizontalMovement = Vector3.zero;
        _rigidBody.linearVelocity = Vector3.zero;
        _directionInput = Vector3.zero;

        if (macroStateMachine.CurrentState == liquidoState)
        {
            ToggleMacroState();
        }
        solidoState.ChangeSubState(solidoState.IdleState);
    }

    #region Inputs
    internal void GetDirectionInput(Vector3 direction)
    {
        _directionInput = direction;
    }

    internal void GetJumpInput(InputInfo info)
    {
        if(IsGrounded)
        {
            _lastJumpInputOnGround = Time.time;
        }
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
    public Rigidbody RigidBody => _rigidBody;
    public PlayerController PlayerController => playerController;
    public float SolidSpeed
    {
        get
        {
            switch (playerController.CurrentMaterial)
            {
                case SurfaceMaterial.Vines:
                    return solidMoveSpeedVines;
                case SurfaceMaterial.Stone:
                    return solidMoveSpeedStone;
                case SurfaceMaterial.Earth:
                    return solidMoveSpeedEarth;
                default:
                    return solidMoveSpeedStone;
            }
        }
    } 
    public StateMachine MacroStateMachine => macroStateMachine;
    public float LiquidSpeed
    {
        get
        {
            switch (playerController.CurrentMaterial)
            {
                case SurfaceMaterial.Vines:
                    return liquidMoveSpeedVines;
                case SurfaceMaterial.Stone:
                    return liquidMoveSpeedStone;
                case SurfaceMaterial.Earth:
                    return liquidMoveSpeedEarth;
                default:
                    return liquidMoveSpeedStone;
            }
        }
    }
    public float SolidJump => solidJumpForce;
    public float LiquidJump => liquidJumpForce;
    public float LastTimeOnGround => playerController.LastTimeOnGround;
    public float LastJumpInputOnGround { get { return _lastJumpInputOnGround; } set { _lastJumpInputOnGround = value; } }
    public bool IsGrounded => (
        (surfaceDetection.CurrentSurface.HasValue && surfaceDetection.CurrentSurface.Value.type == SurfaceType.Floor) ||
        (macroStateMachine.CurrentState == liquidoState) && surfaceDetection.CurrentSurface.HasValue);
    public bool IsGoingDown
    {
        get { return Vector3.Dot(verticalVelocity.normalized, _gravityDirection) <= 0.2f; }
    }
    public Vector3 VerticalVelocity => verticalVelocity;
    public Vector3 CurrentVelocity => _rigidBody.linearVelocity;
    public Vector3 GravityDirection => _gravityDirection;
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
    public Vector3 DirectionInputNormal
    {
        get
        {
            Vector3 normal = playerController.SurfaceDetection.CurrentSurface.Value.hit.normal;
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;

            // Remove componente na direção da normal da superfície
            //camForward = Vector3.ProjectOnPlane(camForward, normal).normalized;
            camRight = Vector3.ProjectOnPlane(camRight, normal).normalized;
            camForward = Vector3.Cross(camRight, normal);

            // Input relativo à câmera, mas em plano da superfície
            Vector3 moveDir = camForward * _directionInput.z + camRight * _directionInput.x;

            // Normaliza para evitar valores maiores que 1
            if (moveDir.sqrMagnitude > 0.001f)
                moveDir.Normalize();

            return moveDir;
        }
    }
    public InputInfo JumpInput => _jumpInput;
    public InputInfo TransformInput => _transformInput;
    public Transform Orientation => playerController.Orientation;
    public bool CoyoteTime => LastTimeOnGround + JumpInput.BufferTime > Time.time;
    public bool CanJump { get { return JumpInput.GetDelayInput(LastJumpInputOnGround) || IsGrounded || (CoyoteTime && _jumpInput.IsDown); } private set { } }
    public bool DidJump { get { return _didJump; } set { _didJump = value; } }
    #endregion
}
#endregion
