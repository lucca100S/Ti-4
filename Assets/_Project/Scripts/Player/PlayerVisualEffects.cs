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

        private ParticleSystem _currentWalkParticle;
        private ParticleSystem _currentJumpParticle;

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
            if (_currentWalkParticle == null)
                return;
            _currentWalkParticle.Play();
        }

        private void HandlePlayerJump()
        {
            if (_currentWalkParticle == null || _currentJumpParticle == null)
                return;
            _currentWalkParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            _currentJumpParticle.Play();
        }

        private void HandleFormChanged(IState state)
        {
            Debug.Log("[PlayerVisualEffects] Form changed to " + state.GetType().Name);
            if (state is SolidoState)
            {
                _currentJumpParticle = _solidJumpParticle;
                _currentWalkParticle = _solidWalkParticle;
            }
            else if (state is LiquidoState)
            {
                _currentJumpParticle = _liquidJumpParticle;
                _currentWalkParticle = _liquidWalkParticle;
            }

            if (!_playerState.IsGrounded && _currentWalkParticle != null)
            {
                _currentWalkParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }
}
