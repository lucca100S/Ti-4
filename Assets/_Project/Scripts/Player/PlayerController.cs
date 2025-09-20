using Lugu.Utils.Debug;
using Player.Movement;
using Player.StateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using static SurfaceDetection;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerInput), typeof(SurfaceDetection))]
    public class PlayerController : MonoBehaviour
    {
        private SurfaceDetection _surfaceDetection;
        private PlayerMovement _playerMovement;
        private PlayerInput _playerInput;

        private State _previousState = State.Air;
        private State _currentState = State.Air;

        private SurfaceMaterial _previousMaterial = SurfaceMaterial.None;
        private SurfaceMaterial _currentMaterial = SurfaceMaterial.None;

        [SerializeField] private List<PlayerState> _statesList;

        public enum State
        {
            Air,
            Ground,
            Wall
        }


        #region Properties

        public SurfaceDetection SurfaceDetection { get { return _surfaceDetection; } private set { _surfaceDetection = value; } }
        public PlayerMovement PlayerMovement { get => _playerMovement; private set => _playerMovement = value; }
        public PlayerInput PlayerInput { get => _playerInput; private set => _playerInput = value; }
        public State PreviousState { get { return _previousState; } private set { _previousState = value; } }
        public State CurrentState { get { return _currentState; } private set { _currentState = value; } }

        public SurfaceMaterial CurrentMaterial { get { return _currentMaterial; } private set { _currentMaterial = value; } }
        public SurfaceMaterial PreviousMaterial { get { return _previousMaterial; } private set { _previousMaterial = value; } }

        #endregion

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerInput = GetComponent<PlayerInput>();
            _surfaceDetection = GetComponent<SurfaceDetection>();
        }

        private void Update()
        {
            _statesList[(int)_currentState].StateUpdate(this);
        }

        private void OnEnable()
        {
            _playerInput.OnMove += _playerMovement.GetDirectionInput;
            _playerInput.JumpInput.OnInput += _playerMovement.GetJumpInput;
            _playerInput.TransformInput.OnInput += _playerMovement.GetTransformInput;

            _surfaceDetection.OnSurfaceChange += OnSurfaceChange;
            _surfaceDetection.OnSurfaceNull += OnSurfaceNull;
            _surfaceDetection.OnSurfaceHit += OnSurfaceHit;
        }

        private void OnDisable()
        {
            _playerInput.OnMove -= _playerMovement.GetDirectionInput;
            _playerInput.JumpInput.OnInput -= _playerMovement.GetJumpInput;
            _playerInput.TransformInput.OnInput -= _playerMovement.GetTransformInput;

            _surfaceDetection.OnSurfaceChange -= OnSurfaceChange;
            _surfaceDetection.OnSurfaceNull -= OnSurfaceNull;
            _surfaceDetection.OnSurfaceHit -= OnSurfaceHit;
        }

        #region StateMachine

        private void OnSurfaceChange(SurfaceDetection.SurfaceHit hit)
        {
            DebugLogger.Log($"[PlayerSurfaceChange] {hit.type}", "PlayerControllerDetection");
        }

        private void OnSurfaceHit(SurfaceDetection.SurfaceHit hit)
        {
            DebugLogger.Log($"[PlayerSurfaceHit] {hit.type}", "PlayerControllerDetection");
            switch (hit.type)
            {
                case SurfaceType.Floor:
                    if (hit.hit.distance < _statesList[(int)State.Ground].DistanceTreshold && _playerMovement.Force.y <= 0)
                    {
                        ChangeState(State.Ground);
                    }
                    else
                    {
                        ChangeState(State.Air);
                    }

                    if (_currentState != State.Air)
                    {
                        ChangeMaterial(hit.material);
                    }
                    break;
                default:
                    ChangeMaterial(hit.material);
                    break;
            }

        }

        private void OnSurfaceNull()
        {
            DebugLogger.Log($"[PlayerOnAir]", "PlayerControllerDetection");
            ChangeState(State.Air);
        }

        private void ChangeState(State state)
        {
            if (state != _currentState)
            {
                _previousState = _currentState;
                _statesList[(int)_previousState].Exit(this);

                _currentState = state;
                _statesList[(int)_currentState].Enter(this);

                DebugLogger.Log($"[PlayerControllerState] {state}", "PlayerControllerState");
            }
        }

        internal void ChangeMaterial(SurfaceMaterial material)
        {
            if (material != _currentMaterial)
            {
                _previousMaterial = _currentMaterial;
                _currentMaterial = material;
                DebugLogger.Log($"[PlayerMaterialChange] {material}", "PlayerMaterialChange");
            }
        }

        #endregion

    }
}
