using Player.Strategy;
using System;
using UnityEngine;

namespace Surfaces
{
    [RequireComponent(typeof(Collider))]
    public class VinesHandler : MonoBehaviour
    {
        private Collider _collider;
        [SerializeField] private bool _isPasstrough = true;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnFormChanged += OnFormChanged;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnFormChanged -= OnFormChanged;
        }

        private void OnFormChanged(PlayerStrategyHandler.Strategy strategy)
        {
            switch(strategy)
            {
                case PlayerStrategyHandler.Strategy.Mud:
                    _collider.enabled = !_isPasstrough;
                    break;
                case PlayerStrategyHandler.Strategy.Solid:
                    _collider.enabled = true;
                    break;
            }
        }
    }
}
