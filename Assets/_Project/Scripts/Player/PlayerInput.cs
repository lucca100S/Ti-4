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

        [Header("Player Input Infos")]
        [SerializeField] private InputInfo _jumpInput;
        [SerializeField] private InputInfo _transformInput;
        private UIManager _uiManager;
        #region Properties
        public Action<Vector3> OnMove { get; set; }
        public InputInfo JumpInput => _jumpInput;
        public InputInfo TransformInput => _transformInput;
        #endregion
        private void Awake()
        {
            _inputActions = new InputActions();
            _playerActions = _inputActions.Player;
            if (_uiManager == null)
            {
                _uiManager = FindAnyObjectByType<UIManager>();

                if (_uiManager == null)
                    Debug.LogError("<color=#eb34cf>[PlayerInput]</color> UIManager não foi encontrado na cena!");
            }

            EnableAllInputs();
        }

        private void OnDestroy()
        {
            DisableAllInputs();
        }

        private void SubscribeMovement()
        {
            _playerActions.Move.performed += MovePerformed;
            _playerActions.Move.canceled += MovePerformed;
            _playerActions.Jump.performed += _jumpInput.GetInput;
            _playerActions.Jump.canceled += _jumpInput.GetInput;
            _playerActions.Transform.performed += _transformInput.GetInput;
            _playerActions.Transform.canceled += _transformInput.GetInput;
        }
        private void UnsubscribeMovement()
        {
            _playerActions.Move.performed -= MovePerformed;
            _playerActions.Move.canceled -= MovePerformed;
            _playerActions.Jump.performed -= _jumpInput.GetInput;
            _playerActions.Jump.canceled -= _jumpInput.GetInput;
            _playerActions.Transform.performed -= _transformInput.GetInput;
            _playerActions.Transform.canceled -= _transformInput.GetInput;
        }
        private void SubscribeUI()
        {
            _playerActions.Pause.performed += ctx => _uiManager.HandleEscPress();
        }

        private void UnsubscribeUI()
        {
            _playerActions.Pause.performed -= ctx => _uiManager.HandleEscPress();

        }
        private void SubscribeCheats()
        {
            _playerActions.ChangeToLevel01.performed += ctx => Player.PlayerCheats.GoToScene(Player.PlayerCheats.level01Name);
            _playerActions.ChangeToLevel01.canceled += ctx => Player.PlayerCheats.GoToScene(Player.PlayerCheats.level01Name);

            _playerActions.ChangeToLevel02.performed += ctx => Player.PlayerCheats.GoToScene(Player.PlayerCheats.level02Name);
            _playerActions.ChangeToLevel02.canceled += ctx => Player.PlayerCheats.GoToScene(Player.PlayerCheats.level02Name);

            _playerActions.ChangeToLevel03.performed += ctx => Player.PlayerCheats.GoToScene(Player.PlayerCheats.level03Name);
            _playerActions.ChangeToLevel03.canceled += ctx => Player.PlayerCheats.GoToScene(Player.PlayerCheats.level03Name);
        }
        private void UnsubscribeCheats()
        {
            _playerActions.ChangeToLevel01.performed -= ctx => Player.PlayerCheats.GoToScene(Player.PlayerCheats.level01Name);
            _playerActions.ChangeToLevel01.canceled -= ctx => Player.PlayerCheats.GoToScene(Player.PlayerCheats.level01Name);

            _playerActions.ChangeToLevel02.performed -= ctx => Player.PlayerCheats.GoToScene(Player.PlayerCheats.level02Name);
            _playerActions.ChangeToLevel02.canceled -= ctx => Player.PlayerCheats.GoToScene(Player.PlayerCheats.level02Name);

            _playerActions.ChangeToLevel03.performed -= ctx => Player.PlayerCheats.GoToScene(Player.PlayerCheats.level03Name);
            _playerActions.ChangeToLevel03.canceled -= ctx => Player.PlayerCheats.GoToScene(Player.PlayerCheats.level03Name);
        }

        public void EnableMovementInputs() { _playerActions.Enable(); SubscribeMovement(); }
        public void DisableMovementInputs() { UnsubscribeMovement(); }
        public void EnableUIInputs() { SubscribeUI(); }
        public void DisableUIInputs() { UnsubscribeUI(); }
        public void EnableCheatInputs() { SubscribeCheats(); }
        public void DisableCheatInputs() { UnsubscribeCheats(); }
        public void EnableAllInputs()
        {
            _playerActions.Enable();
            SubscribeMovement();
            SubscribeUI();
            SubscribeCheats();
        }
        public void DisableAllInputs()
        {
            UnsubscribeMovement();
            UnsubscribeUI();
            UnsubscribeCheats();
            _playerActions.Disable();
        }
        private void MovePerformed(InputAction.CallbackContext context)
        {
            Vector2 move = context.ReadValue<Vector2>();
            OnMove?.Invoke(new Vector3(move.x, 0, move.y));
        }
    }
}
