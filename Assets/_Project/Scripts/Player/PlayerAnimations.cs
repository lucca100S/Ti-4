using System;
using UnityEngine;

namespace Player
{
    public class PlayerAnimations : MonoBehaviour
    {
        [SerializeField] private Animator _mudAnimator;
        [SerializeField] private Animator _solidAnimator;

        private Animator _currentAnimator;

        private void OnEnable()
        {
            ActionsManager.Instance.OnFormChanged += ChangeAnimator;
            ActionsManager.Instance.OnStateChanged += ChangeState;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnFormChanged -= ChangeAnimator;
            ActionsManager.Instance.OnStateChanged -= ChangeState;
        }

        private void Awake()
        {
            _currentAnimator = _solidAnimator;
        }

        public void ChangeAnimator(IState state)
        {
            if (state is LiquidoState)
                _currentAnimator = _mudAnimator;
            else if (state is SolidoState)
                _currentAnimator = _solidAnimator;
        }

        private void ChangeState(StateType state)
        {

            switch(state)
            {
                case StateType.Idle:
                    _currentAnimator.SetTrigger("Idle");
                    break;
                case StateType.Walk:
                    _currentAnimator.SetTrigger("Walk");
                    break;
                case StateType.Jump:
                    _currentAnimator.SetTrigger("Jump");
                    break;
                case StateType.Climb:
                    //_currentAnimator.SetTrigger("Climb");
                    break;
                case StateType.WallJump:
                    //_currentAnimator.SetTrigger("WallJump");
                    break;
            }
        }

    }
}