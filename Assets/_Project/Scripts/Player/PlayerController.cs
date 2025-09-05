using Player.Movement;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {

        private PlayerMovement _playerMovement;
        private PlayerInput _playerInput;

        #region Properties

        public PlayerMovement PlayerMovement { get => _playerMovement; private set => _playerMovement = value; }
        public PlayerInput PlayerInput { get => _playerInput; private set => _playerInput = value; }

        #endregion

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _playerInput.OnMove += _playerMovement.ChangeDirection;
        }

        private void OnDisable()
        {
            _playerInput.OnMove -= _playerMovement.ChangeDirection;
        }

    }
}
