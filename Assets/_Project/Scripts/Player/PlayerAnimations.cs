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
            ActionsManager.Instance.OnStateAnimationChanged += ChangeStateAnimation;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnFormChanged -= ChangeAnimator;
            ActionsManager.Instance.OnStateChanged -= ChangeState;
            ActionsManager.Instance.OnStateAnimationChanged -= ChangeStateAnimation;
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

            _currentAnimator.ResetTrigger("Idle");
            _currentAnimator.ResetTrigger("Walk");
            _currentAnimator.ResetTrigger("Jump");
            _currentAnimator.ResetTrigger("Climb");
            _currentAnimator.ResetTrigger("WallJump");

            switch (state)
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

        private void ChangeStateAnimation(string stateAnimation, float transitionDuration = 0.2f)
        {
            if(string.IsNullOrEmpty(stateAnimation))
                return;
            if (_currentAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateAnimation))
                return;

            _currentAnimator.CrossFade(stateAnimation, transitionDuration);
        }


    }
}