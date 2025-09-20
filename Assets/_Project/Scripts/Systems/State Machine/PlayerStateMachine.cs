using UnityEngine;

#region Player - State Machine (Root)
/// <summary>
/// Controlador principal do player. Mant�m a m�quina de macroestados e gerencia movimento f�sico.
/// - Integra SurfaceDetection (inje��o via inspector)
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
    [Tooltip("Velocidade no modo s�lido (unidades/s)")]
    [SerializeField] public float solidMoveSpeed = 6f;
    [Tooltip("Velocidade no modo l�quido (unidades/s)")]
    [SerializeField] public float liquidMoveSpeed = 4f;
    [Tooltip("For�a de pulo s�lido")]
    [SerializeField] public float solidJumpForce = 8f;
    [Tooltip("For�a de pulo l�quido")]
    [SerializeField] public float liquidJumpForce = 6f;
    [Tooltip("Gravidade aplicada ao player (negativo)")]
    [SerializeField] public float gravity = -20f;
    #endregion

    #region Internal State
    private CharacterController controller;
    private StateMachine macroStateMachine;
    private SolidoState solidoState;
    private LiquidoState liquidoState;

    // Movement composition (os subestados definem este vetor cada frame)
    private Vector3 accumulatedHorizontalMovement = Vector3.zero;
    private float verticalVelocity = 0f;
    private const float terminalVelocity = -50f;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        macroStateMachine = new StateMachine();

        // criar macros e passar depend�ncias
        solidoState = new SolidoState(this, surfaceDetection);
        liquidoState = new LiquidoState(this, surfaceDetection);
    }

    private void Start()
    {
        macroStateMachine.ChangeState(solidoState);
    }

    private void Update()
    {
        // Toggle macro state
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToggleMacroState();
        }

        // Gravity & movement commit (reseta accumulated horizontal a cada frame)
        ApplyGravity();

        // Update state machine (substates v�o chamar SetMovement / AddJump conforme necess�rio)
        macroStateMachine.Update();

        // Move CharacterController com o movimento acumulado
        Vector3 totalMove = accumulatedHorizontalMovement + Vector3.up * verticalVelocity;
        controller.Move(totalMove * Time.deltaTime);

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

    /// <summary>Exposi��o para poss�veis usos (logs/inspector).</summary>
    public string CurrentMacroName => macroStateMachine.CurrentState?.GetType().Name ?? "None";
    #endregion

    #region Gravity Helpers
    private void ApplyGravity()
    {
        // Se est� sobre piso detect�vel e indo para baixo, zera vertical
        if (surfaceDetection != null && surfaceDetection.CurrentSurface.HasValue &&
            surfaceDetection.CurrentSurface.Value.type == SurfaceType.Floor && verticalVelocity <= 0f)
        {
            // Mant�m levemente negativo para garantir contato com CharacterController
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
            if (verticalVelocity < terminalVelocity)
                verticalVelocity = terminalVelocity;
        }
    }
    #endregion

    #region Accessors (para estados)
    public SurfaceDetection SurfaceDetection => surfaceDetection;
    public CharacterController CharacterController => controller;
    public float SolidSpeed => solidMoveSpeed;
    public float LiquidSpeed => liquidMoveSpeed;
    public float SolidJump => solidJumpForce;
    public float LiquidJump => liquidJumpForce;
    #endregion
}
#endregion
