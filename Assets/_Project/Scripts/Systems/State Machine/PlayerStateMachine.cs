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
    [Tooltip("Força de pulo líquido")]
    [SerializeField] public float liquidJumpForce = 6f;
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
    private float verticalVelocity = 0f;
    private const float terminalVelocity = -50f;
    private Vector3 _gravityDirection = Vector3.up;

    #endregion

    private Vector3 _directionInput = Vector3.zero;
    private InputInfo _jumpInput = new InputInfo { };
    private InputInfo _transformInput = new InputInfo { };
    private Vector3 _currentVelocity = Vector3.zero;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _deceleration = 10f;
    [SerializeField] private float _airDeceleration = 10f;

    #region Unity Callbacks
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        _rigidBody = GetComponentInChildren<Rigidbody>();
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
        Vector3 totalMove = accumulatedHorizontalMovement;

        _currentVelocity = Vector3.MoveTowards(_currentVelocity, totalMove, (totalMove.magnitude > _currentVelocity.magnitude ? _acceleration : IsGrounded ? _deceleration : _airDeceleration) * Time.deltaTime);

        _rigidBody.linearVelocity = (_currentVelocity + (_gravityDirection * verticalVelocity));

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

    internal void ApplyGravity()
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
    public bool IsGrounded => (surfaceDetection.CurrentSurface.HasValue && surfaceDetection.CurrentSurface.Value.type == SurfaceType.Floor);
    public float VerticalVelocity => verticalVelocity;
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
    public InputInfo JumpInput => _jumpInput;
    public InputInfo TransformInput => _transformInput;
    public Transform Orientation => playerController.Orientation;

    public bool CanJump { get { return JumpInput.GetDelayInput(LastTimeOnGround) || IsGrounded; } private set { } }
    #endregion
}
#endregion
