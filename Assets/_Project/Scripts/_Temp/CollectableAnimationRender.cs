using UnityEngine;
using DG.Tweening;

public class CollectableAnimationRender : MonoBehaviour
{
    [SerializeField] private float _spinDuration = 1;
    [SerializeField] private float _bounceHeight = 0.5f;
    [SerializeField] private float _bounceDuration = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 360, 0), _spinDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);

        transform.DOMoveY(transform.position.y + _bounceHeight, _bounceDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
