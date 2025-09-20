using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] protected InputActions inputActions;

        private void Awake()
        {
            inputActions = new InputActions();
            inputActions.Player.Enable();
        }

    }
}
