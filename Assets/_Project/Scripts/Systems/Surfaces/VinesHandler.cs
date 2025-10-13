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

        private void OnFormChanged(IState state)
        {
            if(state is SolidoState)
            {
                _collider.enabled = true;
            }
            else if(state is LiquidoState)
            {
                _collider.enabled = !_isPasstrough;
            }
        }
    }
}
