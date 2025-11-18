using System.Collections;
using UnityEngine;

public class PlayerTransformationGroundVFX : MonoBehaviour
{
    [SerializeField] private float _transformDuration = 1.0f;
    [SerializeField] private float _animationSpeedFactor = 1.0f;
    [SerializeField] private Animator _animator;

    private Transform _parent;

    public void Play()
    {
        if (transform.parent.parent != null)
        {
            _parent = transform.parent.parent;
            transform.parent.parent = null;
        }

        transform.parent.position = _parent.position;
        transform.parent.rotation = _parent.rotation;

        StopAllCoroutines();
        _animator.SetFloat("Speed", _animationSpeedFactor);
        _animator.Play("Transform", 0, 0.3f);
        StartCoroutine(DisableVFX());
    }

    private IEnumerator DisableVFX()
    {
        yield return new WaitForSeconds(_transformDuration);
        _animator.gameObject.SetActive(false);
    }

}
