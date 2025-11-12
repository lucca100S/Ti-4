using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace Player
{
    public class PlayerAnimations : MonoBehaviour
    {
        //[SerializeField] private Animator _mudAnimator;
        [SerializeField] private Animator _solidAnimator;
        [SerializeField] private PlayerTransformationGroundVFX _transformationVFX;
        [SerializeField] private PlayerStateMachine _playerStateMachine;

        private Animator _currentAnimator;

        private void OnEnable()
        {
            ActionsManager.Instance.OnFormChanged += ChangeAnimator;
            ActionsManager.Instance.OnStateChanged += ChangeState;
            ActionsManager.Instance.OnStateAnimationChanged += ChangeStateAnimation;
            ActionsManager.Instance.OnAnimatorFloatChanged += ChangeFloat;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnFormChanged -= ChangeAnimator;
            ActionsManager.Instance.OnStateChanged -= ChangeState;
            ActionsManager.Instance.OnStateAnimationChanged -= ChangeStateAnimation;
            ActionsManager.Instance.OnAnimatorFloatChanged -= ChangeFloat;
        }



        private void Awake()
        {
            _currentAnimator = _solidAnimator;
        }

        public void ChangeAnimator(IState state)
        {
            if (!_currentAnimator.gameObject.activeSelf)
                _currentAnimator.gameObject.SetActive(true);
            _currentAnimator.SetBool("Transforming", true);

            if (_playerStateMachine.IsGrounded)
            {
                PlayGroundedTransformation();
            }
            else
            {

            }

            if (state is LiquidoState)
            {
                ChangeStateAnimation("ToLiquid", 0);
            }
            else if (state is SolidoState)
            {
                ChangeStateAnimation("ToSolid", 0);
            }

            StopAllCoroutines();
            StartCoroutine(DisableTransformation(0.2f, state));
        }

        private void PlayGroundedTransformation()
        {
            _transformationVFX.gameObject.SetActive(true);
            _transformationVFX.Play();
        }

        private void ChangeState(StateType state)
        {
            if (!_currentAnimator.gameObject.activeSelf)
                return;

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
                    _currentAnimator.SetTrigger("Climb");
                    break;
                case StateType.WallJump:
                    //_currentAnimator.SetTrigger("WallJump");
                    break;
                case StateType.Transform:
                    _currentAnimator.SetTrigger("Transform");
                    break;
            }


        }

        private void ChangeStateAnimation(string stateAnimation, float transitionDuration = 0.2f)
        {
            if (!_currentAnimator.gameObject.activeSelf)
                return;

            if (string.IsNullOrEmpty(stateAnimation))
                return;
            if (_currentAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateAnimation))
                return;

            _currentAnimator.CrossFade(stateAnimation, transitionDuration);
        }

        private void ChangeFloat(string animatorFloat, float value)
        {
            if (!_currentAnimator.gameObject.activeSelf)
                return;

            _currentAnimator.SetFloat(animatorFloat, value);
        }

        private IEnumerator DisableTransformation(float time, IState newState)
        {
            yield return new WaitForSeconds(time);
            _currentAnimator.SetBool("Transforming", false);
            ActionsManager.Instance.OnTransformAnimationEnded?.Invoke(newState);
        }

    }
}