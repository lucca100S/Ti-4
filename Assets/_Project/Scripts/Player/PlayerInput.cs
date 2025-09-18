using System;
using Systems.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] protected InputActions _inputActions;
        private InputActions.PlayerActions _playerActions;

        [SerializeField] private InputInfo _jumpInput;
        [SerializeField] private InputInfo _transformInput;


        #region Properties

        public Action<Vector3> OnMove { get; set; }
        public InputInfo JumpInput 
        {
            get { return _jumpInput; }
            private set { _jumpInput = value; } 
        }
        public InputInfo TransformInput
        {
            get { return _transformInput; }
            private set { _transformInput = value; }
        }

        #endregion

        private void Awake()
        {
            _inputActions = new InputActions();
            _playerActions = _inputActions.Player;
            _playerActions.Enable();
            _playerActions.Move.performed += MovePerformed;
            _playerActions.Move.canceled += MovePerformed;
            
            _playerActions.Jump.performed += _jumpInput.GetInput;
            _playerActions.Jump.canceled += _jumpInput.GetInput;

            _playerActions.Transform.performed += _transformInput.GetInput;
            _playerActions.Transform.canceled += _transformInput.GetInput;
        }

        private void MovePerformed(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            OnMove?.Invoke(new Vector3(moveInput.x, 0, moveInput.y));
        }
    }
}
