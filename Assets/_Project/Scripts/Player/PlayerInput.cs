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

        private void Awake()
        {
            inputActions = new InputActions();
            playerActions = inputActions.Player;
            playerActions.Enable();
            playerActions.Move.performed += MovePerformed;
            playerActions.Move.canceled += MovePerformed;
        }

        private void MovePerformed(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            OnMove?.Invoke(new Vector3(moveInput.x, 0, moveInput.y));
        }
    }
}
