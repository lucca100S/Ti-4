using Lugu.Utils.Debug;
using Player.Movement;
using Player.StateMachine;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerStateMachine), typeof(PlayerInput), typeof(SurfaceDetection))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CinemachineInputAxisController _cameraInputs;
        [SerializeField] private Transform _orientation;

        private SurfaceDetection _surfaceDetection;
        private PlayerStateMachine _playerStateMachine;
        private PlayerInput _playerInput;
        private Rigidbody _rigidBody;

        private State _previousState = State.Air;
        private State _currentState = State.Air;

        private SurfaceMaterial _previousMaterial = SurfaceMaterial.None;
        private SurfaceMaterial _currentMaterial = SurfaceMaterial.None;

        public enum State
        {
            Air,
            Ground,
            Wall
        }


        #region Properties

        public SurfaceDetection SurfaceDetection { get { return _surfaceDetection; } private set { _surfaceDetection = value; } }
        public PlayerStateMachine PlayerStateMachine { get => _playerStateMachine; private set => _playerStateMachine = value; }
        public PlayerInput PlayerInput { get => _playerInput; private set => _playerInput = value; }
        public State PreviousState { get { return _previousState; } private set { _previousState = value; } }
        public State CurrentState { get { return _currentState; } private set { _currentState = value; } }

        public SurfaceMaterial CurrentMaterial { get { return _currentMaterial; } private set { _currentMaterial = value; } }
        public SurfaceMaterial PreviousMaterial { get { return _previousMaterial; } private set { _previousMaterial = value; } }

        public float LastTimeOnGround { get; private set; }
        public Transform Orientation { get { return _orientation; } private set { _orientation = value; } }

        #endregion

        private void Awake()
        {
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            _playerInput = GetComponent<PlayerInput>();
            _surfaceDetection = GetComponent<SurfaceDetection>();
            _rigidBody = GetComponentInChildren<Rigidbody>();

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            _playerInput.OnMove += _playerStateMachine.GetDirectionInput;
            _playerInput.JumpInput.OnInput += _playerStateMachine.GetJumpInput;
            _playerInput.TransformInput.OnInput += _playerStateMachine.GetTransformInput;

            _surfaceDetection.OnSurfaceChange += OnSurfaceChange;
            _surfaceDetection.OnSurfaceNull += OnSurfaceNull;
            _surfaceDetection.OnSurfaceHit += OnSurfaceHit;
        }

        private void OnDisable()
        {
            _playerInput.OnMove -= _playerStateMachine.GetDirectionInput;
            _playerInput.JumpInput.OnInput -= _playerStateMachine.GetJumpInput;
            _playerInput.TransformInput.OnInput -= _playerStateMachine.GetTransformInput;

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
                    /* if (hit.hit.distance < _statesList[(int)State.Ground].DistanceTreshold && _playerMovement.Force.y <= 0)
                    {
                        ChangeState(State.Ground);
                    }
                    else
                    {
                        ChangeState(State.Air);
                    }*/

                    //if (_currentState != State.Air)
                    //{
                        ChangeMaterial(hit.material);
                    //}
                    LastTimeOnGround = Time.time;
                    break;
                case SurfaceType.Wall:
                    //ChangeState(State.Wall);
                    ChangeMaterial(hit.material);
                    break;
                default:
                    ChangeMaterial(hit.material);
                    break;
            }

        }

        private void OnSurfaceNull()
        {
            DebugLogger.Log($"[PlayerOnAir]", "PlayerControllerDetection");
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

        public void ChangeCameraSensitivity(float sensitivity)
        {
            sensitivity = Mathf.Abs(sensitivity);
            foreach (var c in _cameraInputs.Controllers)
            {
                c.Input.Gain = sensitivity * Mathf.Sign(c.Input.Gain);
            }
        }

        internal void ChangeState(State air)
        {
            
        }

        internal void RotateModelTowards(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            _orientation.rotation = Quaternion.Lerp(_orientation.rotation, targetRotation, Time.deltaTime * 10f);
            //_rigidBody.MoveRotation(_orientation.rotation);
        }

        internal void RotateModelTowardsInstant(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            _orientation.rotation = targetRotation;
        }
    }
}
