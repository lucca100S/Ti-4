using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Input
{
    [System.Serializable]
    public class InputInfo
    {

        [SerializeField] private float _bufferTime;
        [SerializeField] private float _delayTime;

        private float _lastInput;

        private bool _isEnabled;

        private bool _isPressed;
        private bool _isUp;
        private bool _isDown;

        private bool _isDelayInput;

        private Awaitable _inputBufferAwaitable;

        #region Properties

        public float LastInput
        {
            get { return _lastInput; }
        }

        public float BufferTime
        {
            get { return _bufferTime; }
        }

        public float DelayTime
        {
            get { return _delayTime; }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
        }

        public bool IsPressed
        {
            get { return _isPressed; }
        }

        public bool IsUp
        {
            get { return _isUp; }
        }

        public bool IsDown
        {
            get { return _isDown; }
        }

        public bool IsDelayInput
        {
            get { return _isDelayInput; }
        }

        public Action<InputInfo> OnInput;

        #endregion

        public InputInfo(float bufferTime = 0.2f, float delayTime = 0.2f)
        {
            _bufferTime = bufferTime;
            _delayTime = delayTime;

            _lastInput = -1;
        }

        public void GetInput(InputAction.CallbackContext context)
        {
            _isPressed = !context.canceled;
            _isUp = context.canceled;
            _isDown = !context.canceled && !_isDown;

            if (_isDown)
            {
                _lastInput = Time.time;
                if(_inputBufferAwaitable != null) _inputBufferAwaitable.Cancel();
                _inputBufferAwaitable = BufferInput();
            }

            OnInput?.Invoke(this);

        }

        public bool GetDelayInput(float time)
        {
            return Time.time <= time + _delayTime && _isDown;
        }

        private async Awaitable BufferInput()
        {
            _isEnabled = true;
            await Awaitable.WaitForSecondsAsync(_bufferTime);
            _isEnabled = false;
        }
    }
}