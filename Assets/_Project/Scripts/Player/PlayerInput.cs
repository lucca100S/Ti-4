using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] protected InputActions inputActions;
        private InputActions.PlayerActions playerActions;

        public Action<Vector3> OnMove { get; set; }
        public Action<bool> OnJump { get; set; }

        private void Awake()
        {
            inputActions = new InputActions();
            playerActions = inputActions.Player;
            playerActions.Enable();
            playerActions.Move.performed += MovePerformed;
            playerActions.Move.canceled += MovePerformed;

            playerActions.Jump.performed += JumpPerformed;
            playerActions.Jump.canceled += JumpPerformed;
        }


        private void MovePerformed(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            OnMove?.Invoke(new Vector3(moveInput.x, 0, moveInput.y));
        }
        private void JumpPerformed(InputAction.CallbackContext context)
        {
            float moveInput = context.ReadValue<float>();
            OnJump?.Invoke(moveInput > 0);
        }
    }
}
