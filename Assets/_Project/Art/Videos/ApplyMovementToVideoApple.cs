using UnityEngine;
using DG.Tweening;


public class ApplyMovementToVideoApple : MonoBehaviour
{
    [SerializeField] private float _spinDuration = 1;
    void Start()
    {
        transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, 360, 0), _spinDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }
}
