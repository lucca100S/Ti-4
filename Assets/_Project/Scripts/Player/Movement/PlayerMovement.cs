using Lugu.Utils.Debug;
using Player.Strategy;
using System;
using Systems.Input;
using UnityEngine;
using static Player.Strategy.PlayerStrategyHandler;

namespace Player.Movement
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerStrategyHandler))]
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerController _controller;

        [SerializeField] private PlayerStrategyHandler.Strategy _startStrategy;
        [SerializeField] private Transform _model;

        private PlayerStrategyScriptable _currentStrategy;

        private CharacterController _characterController;

        private Vector3 _input;
        private bool _canCancelJump = false;
        private bool _isGrounded = false;
        private bool _canJump = false;
        private bool _isWallJumping = false;

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

        public bool CanJump { get { return _canJump; } internal set { _canJump = value; } }

        public CharacterController CharacterController { get { return _characterController; } internal set { _characterController = value; } }
        public SurfaceDetection SurfaceDetection { get { return _controller.SurfaceDetection; } }

        public bool IsGrounded { get { return _isGrounded; } }
        public bool IsWallJumping { get { return _isWallJumping; } internal set { _isWallJumping = value; } }

        public PlayerController.State CurrentState { get { return _controller.CurrentState; } }
        public SurfaceMaterial CurrentMaterial { get { return _controller.CurrentMaterial; } }
        public SurfaceMaterial PreviousMaterial { get { return _controller.PreviousMaterial; } }

        public PlayerController Controller { get { return _controller; } }

        #endregion

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _strategyHandler = GetComponent<PlayerStrategyHandler>();
            _controller = GetComponent<PlayerController>();

            ChangeStrategy(_startStrategy);
            LockMouse();
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            RotateStrategy?.Invoke(this);
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

            switch (CurrentState)
            {
                case PlayerController.State.Ground:
                case PlayerController.State.Air:
                    break;
                case PlayerController.State.Wall:
                    if (_currentStrategy.MaterialStats[(int)CurrentMaterial].CanClimb)
                    {
                        gravity = 0;
                    }
                    else if(_force.y < 0)
                    {
                        gravity /= 2;
                    }
                    break;
            }

            _force.y += gravity * Time.deltaTime;

            if (_isGrounded)
            {
                _lastTimeOnGround = Time.time;
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

            DebugLogger.Log("[PlayerInput] Jump Input", "PlayerInput");

            bool coyoteTimeEnabled = jumpInput.GetDelayInput(_lastTimeOnGround);
            if ((_isGrounded || coyoteTimeEnabled) && _canJump)
            {
                if (jumpInput.IsEnabled || coyoteTimeEnabled)
                {
                    DebugLogger.Log("[PlayerInput] Jumped", "PlayerInput");
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
            if (transformInput.IsDown)
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
            if (CurrentState == PlayerController.State.Air)
                _controller.ChangeMaterial(SurfaceMaterial.None);


            _currentStrategy = _strategyHandler.ChangeStrategy(nextStrategyEnum);

            #region Changing Stats
            _characterController.height = _currentStrategy.Height;
            _characterController.center = _currentStrategy.Center;

            _model.transform.localPosition = _currentStrategy.Center;
            _model.transform.localScale = _currentStrategy.Scale;

            JumpStrategy = _currentStrategy.Jump;
            MoveStrategy = _currentStrategy.Move;
            DirectionStrategy = _currentStrategy.GetDirection;
            TransformStrategy = _currentStrategy.Transform;
            RotateStrategy = _currentStrategy.Rotate;
            #endregion

            ActionsManager.Instance.OnFormChanged?.Invoke(_currentStrategy.Strategy);
        }

        public PlayerStrategyScriptable GetStrategy(PlayerStrategyHandler.Strategy strategy)
        {
            return _strategyHandler.ChangeStrategy(strategy);
        }
        #endregion

        internal void Grounded(bool grounded, bool gravityCancel = true)
        {
            if (grounded && gravityCancel)
            {
                float gravity = _currentStrategy.Gravity * _currentStrategy.FallGravityFactor;
                if (_force.y <= 0) _force.y = gravity;

                _lastTimeOnGround = Time.time;
            }
            _canJump = grounded;
            _isGrounded = grounded;
        }

        public void LockMouse()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
}