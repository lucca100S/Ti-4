using Player;
using Player.Movement;
using System;
using UnityEngine;

namespace Player
{
    public class PlayerVisualEffects : MonoBehaviour
    {
        private PlayerStateMachine _playerState;
        private PlayerController _playerController;

        [Header("Solid Effects")]
        [SerializeField] private ParticleSystem _solidWalkParticle;
        [SerializeField] private ParticleSystem _solidJumpParticle;

        [Header("Liquid Effects")]
        [SerializeField] private ParticleSystem _liquidWalkParticle;
        [SerializeField] private ParticleSystem _liquidJumpParticle;

        private void Awake()
        {
            _playerState = GetComponent<PlayerStateMachine>();
            _playerController = GetComponent<PlayerController>();
        }

        private void Start()
        {
            HandleFormChanged(_playerState.MacroStateMachine.CurrentState);
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnFormChanged += HandleFormChanged;
            ActionsManager.Instance.OnPlayerJumped += HandlePlayerJump;
            ActionsManager.Instance.OnPlayerLanded += HandlePlayerLand;

        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnFormChanged -= HandleFormChanged;
            ActionsManager.Instance.OnPlayerJumped -= HandlePlayerJump;
            ActionsManager.Instance.OnPlayerLanded -= HandlePlayerLand;
        }

        private void HandlePlayerLand()
        {
            if((_playerState.MacroStateMachine.CurrentState is SolidoState))
            {
                _solidWalkParticle.Play();
            }
            else
            {

            }

        }

        private void HandlePlayerJump()
        {
            bool hasValue = _playerController.SurfaceDetection.CurrentSurface.HasValue;
            bool isOnWall = hasValue ? _playerController.SurfaceDetection.CurrentSurface.Value.type == SurfaceType.Wall : false;

            _solidWalkParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            if ((_playerState.MacroStateMachine.CurrentState is SolidoState))
            {
                if (!isOnWall)
                {
                    _solidJumpParticle.Play();
                }
            }
            else
            {

            }

            
           
        }

        private void HandleFormChanged(IState state)
        {
            if (state is SolidoState)
            {
                _solidWalkParticle.Play();
            }
            else if (state is LiquidoState)
            {
                _solidWalkParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            if (!_playerState.IsGrounded)
            {
                _solidWalkParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }
}
