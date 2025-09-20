using Player.Strategy;
using System;
using Systems.Input;
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
        private State _currentState = State.Idle;

        [SerializeField] private PlayerStrategyHandler.Strategy _startStrategy;
        private PlayerStrategyScriptable _currentStrategy;

        private CharacterController _characterController;

        private Vector3 _input;
        private bool _canCancelJump = false;
        private bool _isGrounded = false;
        private bool _canJump = false;

        private Vector3 _force;
        private Vector3 _direction;

        #region Strategy Actions
        private PlayerStrategyHandler _strategyHandler;

        private Action<PlayerMovement> JumpStrategy;
        private Action<PlayerMovement> MoveStrategy;
        private Action<PlayerMovement> TransformStrategy;
        private Action<PlayerMovement> DirectionStrategy;
        private Action<PlayerMovement> RotateStrategy;
        #endregion

        private float _lastTimeOnGround;

        #region Properties

        public Vector3 Input
        {
            get { return _input; }
            private set { _input = value; }
        }
        public Vector3 Force
        {
            get { return _force; }
            internal set { _force = value; }
        }
        public Vector3 Direction
        {
            get { return _direction; }
            internal set { _direction = value; }
        }

        public CharacterController CharacterController { get { return _characterController; } internal set { _characterController = value; } }
        public State CurrentState { get { return _currentState; } internal set { _currentState = value; } }

        public bool IsGrounded { get { return _isGrounded; } }

        #endregion

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _strategyHandler = GetComponent<PlayerStrategyHandler>();

            ChangeStrategy(_startStrategy);
            LockMouse();
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            RotateStrategy?.Invoke(this);
        }

        private void LateUpdate()
        {
            CheckGround();
        }

        #region Movement
        private void Move()
        {
            HandleGravity();

            DirectionStrategy?.Invoke(this);
            MoveStrategy?.Invoke(this);

            _characterController.Move(_force * Time.deltaTime);
        }
        internal void HandleGravity()
        {
            float gravity = _currentStrategy.Gravity;

            if (_force.y < 0)
            {
                gravity *= _currentStrategy.FallGravityFactor;
            }

            _force.y += gravity * Time.deltaTime;

            if (_isGrounded)
            {
                _lastTimeOnGround = Time.time;
            }
        }
        private void CheckGround()
        {
            _isGrounded = _characterController.isGrounded;
            _canJump = _characterController.isGrounded;

            if (_isGrounded)
            {
                ChangeState(State.Idle);
                _force.y = 0;
            }
            else
            {
                ChangeState(State.Jumping);
            }
        }
        #endregion

        #region Inputs Handling

        public void GetJumpInput(InputInfo jumpInput)
        {
            if (!jumpInput.IsPressed && _force.y > 0 && _canCancelJump)
            {
                _force.y *= _currentStrategy.JumpCancelFactor;
                _canCancelJump = false;
            }

            bool coyoteTimeEnabled = jumpInput.GetDelayInput(_lastTimeOnGround);
            if ((_isGrounded || coyoteTimeEnabled) && _canJump)
            {
                if (jumpInput.IsEnabled || coyoteTimeEnabled)
                {
                    _canCancelJump = true;
                    JumpStrategy?.Invoke(this);
                    _canJump = false;
                }
                else if (!jumpInput.IsPressed)
                {
                    _canCancelJump = false;
                }
            }
        }
        public void GetTransformInput(InputInfo transformInput)
        {
            if(transformInput.IsDown)
            {
                TransformStrategy?.Invoke(this);
            }
        }
        public void GetDirectionInput(Vector3 direction)
        {
            _input.x = direction.x;
            _input.z = direction.z;
        }

        #endregion

        #region Strategy
        internal void ChangeStrategy(PlayerStrategyHandler.Strategy nextStrategyEnum)
        {
            _currentStrategy = _strategyHandler.ChangeStrategy(nextStrategyEnum);

            _characterController.height = _currentStrategy.Height;

            JumpStrategy = _currentStrategy.Jump;
            MoveStrategy = _currentStrategy.Move;
            DirectionStrategy = _currentStrategy.GetDirection;
            TransformStrategy = _currentStrategy.Transform;
            RotateStrategy = _currentStrategy.Rotate;
        }

        public PlayerStrategyScriptable GetStrategy(PlayerStrategyHandler.Strategy strategy)
        {
            return _strategyHandler.ChangeStrategy(strategy);
        }

        private void ChangeState(State state)
        {
            _currentState = state;
        }
        #endregion
        public void LockMouse()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
}