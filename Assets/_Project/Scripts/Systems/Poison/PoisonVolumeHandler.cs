using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PoisonVolumeHandler : MonoBehaviour
{
    [SerializeField] private Volume _volume;
    [SerializeField] private float _toggleSpeed = 1;

    bool _isEnabled = true;


    public void ToggleVolume()
    {
        _isEnabled = !_isEnabled;
    }

    private void Update()
    {
        float value = _isEnabled ? 1.0f : -1.0f;

        _volume.weight += value * Time.deltaTime * _toggleSpeed;

        _volume.weight = Mathf.Clamp01(value);
    }
}
