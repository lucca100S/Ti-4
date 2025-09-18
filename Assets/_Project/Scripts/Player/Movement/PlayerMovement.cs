using Player.Strategy;
using System;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerStrategyHandler))]
    public class PlayerMovement : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Walking,
            Jumping,
            Falling,
            Climbing
        }
        private State m_currentState = State.Idle;

        [SerializeField] private PlayerStrategyHandler.Strategy m_startStrategy;
        private PlayerStrategyScriptable m_currentStrategy;

        private CharacterController m_characterController;

        private Vector3 m_input;
        private bool m_canCancelJump = false;
        private bool m_isGrounded = false;
        private bool m_canJump = false;

        private Vector3 m_force;
        private Vector3 m_direction;

        [SerializeField] private InputInfo m_jumpInput;
        [SerializeField] private InputInfo m_transformInput;

        //Strategy
        private PlayerStrategyHandler m_strategyHandler;

        private Action<PlayerMovement> JumpStrategy;
        private Action<PlayerMovement> MoveStrategy;
        private Action<PlayerMovement> TransformStrategy;
        private Action<PlayerMovement> DirectionStrategy;
        private Action<PlayerMovement> RotateStrategy;



        private float m_lastTimeOnGround;

        #region Properties

        public Vector3 force
        {
            get { return m_force; }
            internal set { m_force = value; }
        }

        public Vector3 direction
        {
            get { return m_direction; }
            internal set { m_direction = value; }
        }

        public Vector3 input
        {
            get { return m_input; }
            private set { m_input = value; }
        }

        public CharacterController characterController { get { return m_characterController; } internal set { m_characterController = value; } }
        public State currentState { get { return m_currentState; } internal set { m_currentState = value; } }

        public bool isGrounded { get { return m_isGrounded; } }

        #endregion

        private void Awake()
        {
            m_characterController = GetComponent<CharacterController>();
            m_strategyHandler = GetComponent<PlayerStrategyHandler>();

            ChangeStrategy(m_startStrategy);
            LockMouse();
        }

        // Update is called once per frame
        void Update()
        {
            GetInputs();
            Move();
            Jump();
            RotateStrategy?.Invoke(this);
        }

        private void LateUpdate()
        {
            CheckGround();
        }

        private void Move()
        {
            DirectionStrategy?.Invoke(this);
            MoveStrategy?.Invoke(this);
        }

        internal void HandleGravity()
        {
            if (!m_isGrounded)
            {
                float gravity = m_currentStrategy.gravity;

                if (m_force.y < 0)
                {
                    gravity *= m_currentStrategy.fallGravityFactor;
                }

                if (!m_jumpInput.isPressed && m_force.y > 0 && m_canCancelJump)
                {
                    m_force.y *= m_currentStrategy.jumpCancelFactor;
                    m_canCancelJump = false;
                }
                else
                {
                    m_force.y += gravity * Time.deltaTime;
                }
            }
            else if (m_isGrounded)
            {
                m_lastTimeOnGround = Time.time;
            }
        }

        private void Jump()
        {
            bool coyoteTimeEnabled = m_jumpInput.GetDelayInput(m_lastTimeOnGround);
            if ((m_isGrounded || coyoteTimeEnabled) && m_canJump)
            {
                if (m_jumpInput.isEnabled || coyoteTimeEnabled)
                {
                    m_canCancelJump = true;
                    JumpStrategy?.Invoke(this);
                    m_canJump = false;
                }
                else if (!m_jumpInput.isPressed)
                {
                    m_canCancelJump = false;
                }
            }
        }

        private void CheckGround()
        {
            m_isGrounded = m_characterController.isGrounded;
            m_canJump = m_characterController.isGrounded;

            if (m_isGrounded)
            {
                ChangeState(State.Idle);
                m_force.y = 0;
            }
            else
            {
                ChangeState(State.Jumping);
            }
        }

        private void GetInputs()
        {
            m_jumpInput.GetInput();
            m_transformInput.GetInput();

            if (m_transformInput.isDown)
            {
                TransformStrategy?.Invoke(this);
            }

            m_input.x = Input.GetAxis("Horizontal");
            m_input.z = Input.GetAxis("Vertical");
        }

        public void LockMouse()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        internal void ChangeStrategy(PlayerStrategyHandler.Strategy nextStrategyEnum)
        {
            m_currentStrategy = m_strategyHandler.ChangeStrategy(nextStrategyEnum);

            m_characterController.height = m_currentStrategy.height;

            JumpStrategy = m_currentStrategy.Jump;
            MoveStrategy = m_currentStrategy.Move;
            DirectionStrategy = m_currentStrategy.GetDirection;
            TransformStrategy = m_currentStrategy.Transform;
            RotateStrategy = m_currentStrategy.Rotate;
        }
        public PlayerStrategyScriptable GetStrategy(PlayerStrategyHandler.Strategy strategy)
        {
            return m_strategyHandler.ChangeStrategy(strategy);
        }
        private void ChangeState(State state)
        {
            m_currentState = state;
        }

    }
}